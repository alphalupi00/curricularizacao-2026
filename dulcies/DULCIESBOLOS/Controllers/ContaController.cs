
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization; //modificações
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using DULCIESBOLOS.Models;
using DULCIESBOLOS.Models.ViewModel;

namespace DULCIESBOLOS.Controllers
{
    [Authorize] //modificações
    public class ContaController : Controller
    {
        private readonly SignInManager<Usuario> _signInManager;
        private readonly UserManager<Usuario> _userManager;
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
        // GET /Conta/Registro — Exibe o formulário de cadastro de novo Cliente
        // =======================================================================
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Registro(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        // =======================================================================
        // POST /Conta/Registro — Processa o cadastro de novo Cliente
        // =======================================================================
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registro(RegistroViewModel model, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (ModelState.IsValid)
            {
                var usuario = new Usuario
                {
                    UserName = model.Email,
                    Email = model.Email,
                    NomeCompleto = model.NomeCompleto,
                    PhoneNumber = model.Telefone ?? string.Empty,
                    Endereco = model.Endereco ?? string.Empty,
                    EmailConfirmed = true
                };

                var resultado = await _userManager.CreateAsync(usuario, model.Senha);

                if (resultado.Succeeded)
                {
                    // Todo novo usuário registrado pelo formulário recebe automaticamente a Role "Cliente"
                    await _userManager.AddToRoleAsync(usuario, "Cliente");

                    // Realiza o login imediato do cliente recém-cadastrado
                    await _signInManager.SignInAsync(usuario, isPersistent: false);

                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }

                    return RedirectToAction("Index", "Home");
                }

                foreach (var erro in resultado.Errors)
                {
                    ModelState.AddModelError(string.Empty, erro.Description);
                }
            }

            return View(model);
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