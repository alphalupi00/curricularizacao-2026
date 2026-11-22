using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using DULCIESBOLOS.Models;
using DULCIESBOLOS.Models.ViewModel;

namespace DULCIESBOLOS.Controllers
{
    [Authorize]
    public class ContaController : Controller
    {
        private readonly SignInManager<Usuario> _signInManager;
        private readonly UserManager<Usuario> _userManager;

        public ContaController(SignInManager<Usuario> signInManager, UserManager<Usuario> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string senha, bool lembrarMe, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (!ModelState.IsValid) return View();

            var result = await _signInManager.PasswordSignInAsync(email, senha, lembrarMe, lockoutOnFailure: true);

            if (result.Succeeded) return RedirecionarLocal(returnUrl);

            ModelState.AddModelError(string.Empty, result.IsLockedOut
                ? "Conta bloqueada temporariamente por excesso de tentativas. Tente novamente em 5 min."
                : "E-mail ou senha inválidos.");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Registro(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registro(RegistroViewModel model, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (!ModelState.IsValid) return View(model);

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
                await _userManager.AddToRoleAsync(usuario, "Cliente");
                await _signInManager.SignInAsync(usuario, isPersistent: false);
                return RedirecionarLocal(returnUrl);
            }

            foreach (var erro in resultado.Errors)
                ModelState.AddModelError(string.Empty, erro.Description);

            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult AcessoNegado() => View();

        private IActionResult RedirecionarLocal(string? returnUrl) =>
            !string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl)
                ? Redirect(returnUrl)
                : RedirectToAction("Index", "Home");
    }
}