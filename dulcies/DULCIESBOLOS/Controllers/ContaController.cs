// Importa o namespace de Autorização do ASP.NET Core — contém atributos como
// [Authorize] e [AllowAnonymous], que controlam QUEM pode acessar cada action.
using Microsoft.AspNetCore.Authorization;

// Importa o ASP.NET Core Identity — fornece as classes SignInManager e UserManager,
// que são os serviços centrais de segurança para login, logout e gerenciamento de usuários.
using Microsoft.AspNetCore.Identity;

// Importa o framework MVC — fornece a classe base Controller, os atributos [HttpGet],
// [HttpPost], [ValidateAntiForgeryToken] e os métodos View(), Redirect(), etc.
using Microsoft.AspNetCore.Mvc;

// Importa os Models do projeto — precisamos da classe Usuario (que herda de IdentityUser)
// para que o SignInManager e UserManager saibam qual entidade de usuário utilizar.
using DULCIESBOLOS.Models;

namespace DULCIESBOLOS.Controllers
{
    // ============================================================================
    // ContaController — Controller responsável por toda a segurança de acesso:
    // Login, Logout e Acesso Negado.
    // ============================================================================
    public class ContaController : Controller
    {
        // ---------------------------------------------------------------------------
        // SignInManager<Usuario>: Serviço do Identity que gerencia SESSÕES de login.
        // É ele quem cria o cookie de autenticação, valida senha, verifica lockout,
        // e encerra a sessão no logout. Sem ele, não existe login/logout funcional.
        // ---------------------------------------------------------------------------
        private readonly SignInManager<Usuario> _signInManager;

        // ---------------------------------------------------------------------------
        // UserManager<Usuario>: Serviço do Identity que gerencia o CADASTRO de usuários.
        // Permite criar, buscar, atualizar e excluir usuários, verificar senhas,
        // adicionar roles, gerar tokens de reset, etc.
        // ---------------------------------------------------------------------------
        private readonly UserManager<Usuario> _userManager;

        // O construtor recebe os serviços via Injeção de Dependência (DI).
        // O ASP.NET Core resolve automaticamente esses serviços porque foram
        // registrados no Program.cs com AddIdentity<Usuario, IdentityRole>().
        public ContaController(SignInManager<Usuario> signInManager, UserManager<Usuario> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        // =======================================================================
        // GET /Conta/Login — Exibe o formulário de login
        // =======================================================================
        //
        // [HttpGet]: Responde apenas a requisições GET (quando o usuário navega até a página).
        //
        // [AllowAnonymous]: SEGURANÇA — Permite acesso SEM estar logado.
        // Sem isso, se o controller ou a aplicação tiver [Authorize] global,
        // o usuário nunca conseguiria chegar na tela de login (seria um loop infinito).
        //
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string? returnUrl = null)
        {
            // Guarda a URL de retorno para redirecionar o usuário após o login.
            // Exemplo: se tentou acessar /Vendas/Create sem estar logado,
            // o returnUrl será "/Vendas/Create" para voltar lá depois do login.
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        // =======================================================================
        // POST /Conta/Login — Processa a tentativa de login
        // =======================================================================
        //
        // [HttpPost]: Responde apenas a requisições POST (quando o formulário é submetido).
        //
        // [AllowAnonymous]: Mesmo motivo do GET — precisa ser acessível sem autenticação.
        //
        // [ValidateAntiForgeryToken]: SEGURANÇA CONTRA CSRF (Cross-Site Request Forgery).
        // Exige que o formulário envie um token anti-falsificação gerado pelo servidor.
        // Isso impede que um site malicioso forje uma requisição POST em nome do usuário.
        // O token é gerado automaticamente pelo tag helper <form asp-action="Login">.
        //
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string senha, bool lembrarMe, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            // ModelState.IsValid: Verifica se todos os dados recebidos passam nas
            // regras de validação (Data Annotations dos models e validações do formulário).
            if (ModelState.IsValid)
            {
                // ---------------------------------------------------------------
                // PasswordSignInAsync: O COMANDO CENTRAL DE AUTENTICAÇÃO.
                // ---------------------------------------------------------------
                // Parâmetros:
                //   email          → Nome de usuário (neste caso, o e-mail)
                //   senha          → Senha em texto plano (o Identity compara com o hash armazenado)
                //   lembrarMe      → Se true, o cookie persiste após fechar o navegador
                //   lockoutOnFailure: true → SEGURANÇA CONTRA FORÇA BRUTA:
                //       Cada tentativa falha incrementa um contador. Após 5 falhas
                //       (configurado no Program.cs), a conta é bloqueada por 5 minutos,
                //       impedindo ataques automatizados de adivinhação de senha.
                //
                // Internamente o Identity:
                //   1. Busca o usuário pelo e-mail no banco de dados
                //   2. Compara a senha com o hash BCrypt/PBKDF2 armazenado
                //   3. Verifica se a conta está bloqueada (lockout)
                //   4. Se tudo OK, cria um cookie de autenticação criptografado
                // ---------------------------------------------------------------
                var result = await _signInManager.PasswordSignInAsync(email, senha, lembrarMe, lockoutOnFailure: true);

                // Login bem-sucedido — o cookie de autenticação já foi criado
                if (result.Succeeded)
                {
                    // SEGURANÇA: Url.IsLocalUrl() verifica se a URL de retorno é LOCAL.
                    // Isso previne ataques de "Open Redirect", onde um atacante poderia
                    // colocar uma URL externa maliciosa no returnUrl (ex: returnUrl=http://site-falso.com)
                    // para roubar credenciais. Com IsLocalUrl, só aceita URLs do próprio site.
                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    // Se não tem returnUrl válido, vai para a página inicial
                    return RedirectToAction("Index", "Home");
                }

                // SEGURANÇA: Verifica se a conta foi bloqueada por excesso de tentativas.
                // Exibe mensagem clara para o usuário, sem revelar se o e-mail existe ou não.
                if (result.IsLockedOut)
                {
                    ModelState.AddModelError(string.Empty, "Conta bloqueada temporariamente por excesso de tentativas. Tente novamente em 5 minutos.");
                    return View();
                }

                // SEGURANÇA: Mensagem genérica "E-mail ou senha inválidos" — nunca
                // diz "e-mail não encontrado" ou "senha errada" separadamente.
                // Isso impede que um atacante descubra quais e-mails estão cadastrados
                // no sistema (ataque de enumeração de usuários).
                ModelState.AddModelError(string.Empty, "E-mail ou senha inválidos.");
            }

            return View();
        }

        // =======================================================================
        // POST /Conta/Logout — Encerra a sessão do usuário
        // =======================================================================
        //
        // [HttpPost]: Logout é POST (não GET) por segurança.
        // Se fosse GET, um atacante poderia deslogar o usuário simplesmente
        // colocando <img src="/Conta/Logout"> em qualquer página.
        //
        // [ValidateAntiForgeryToken]: Protege contra CSRF no logout também.
        // Sem isso, um site externo poderia forçar o logout do usuário.
        //
        // Nota: NÃO tem [AllowAnonymous] — só usuários logados podem fazer logout.
        //
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            // SignOutAsync: Remove o cookie de autenticação do navegador do usuário,
            // efetivamente encerrando a sessão. Após isso, o usuário volta a ser
            // "anônimo" e precisará fazer login novamente para acessar áreas protegidas.
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        // =======================================================================
        // GET /Conta/AcessoNegado — Página exibida quando o usuário não tem permissão
        // =======================================================================
        //
        // [AllowAnonymous]: Precisa ser acessível sem autenticação, pois o próprio
        // framework redireciona para cá quando um usuário (logado ou não) tenta
        // acessar um recurso que exige uma Role que ele não possui.
        // Configurado no Program.cs como: AccessDeniedPath = "/Conta/AcessoNegado"
        //
        [HttpGet]
        [AllowAnonymous]
        public IActionResult AcessoNegado()
        {
            return View();
        }
    }
}