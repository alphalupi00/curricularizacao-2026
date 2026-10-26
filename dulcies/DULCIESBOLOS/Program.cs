using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using DULCIESBOLOS.Data;
using DULCIESBOLOS.Models;

var builder = WebApplication.CreateBuilder(args);

// Adiciona suporte a Controllers e Views
builder.Services.AddControllersWithViews();

// Registra o AppDBContext com SQL Server
builder.Services.AddDbContext<AppDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configuração dos Serviços do ASP.NET Core Identity
builder.Services.AddIdentity<Usuario, IdentityRole>(options =>
{
    // Regras de Complexidade de Senha
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;

    // Proteção contra Ataques de Força Bruta (Lockout)
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    // Unicidade de E-mail
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<AppDBContext>()
.AddDefaultTokenProviders();

// Configurações de Segurança dos Cookies de Autenticação
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.SameSite = SameSiteMode.Lax;
    options.LoginPath = "/Conta/Login";
    options.AccessDeniedPath = "/Conta/AcessoNegado";
    options.ExpireTimeSpan = TimeSpan.FromDays(7);
    options.SlidingExpiration = true;
});

var app = builder.Build();

// Executa a Carga Inicial Automática (Seed de Perfis e Administrador padrão)
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await DbInitializer.SeedRolesAndAdminAsync(services);
}

// Configuração do pipeline de requisições HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

// SEGURANÇA: Autenticação SEMPRE deve vir antes de Autorização
app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
