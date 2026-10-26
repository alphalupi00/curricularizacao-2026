using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using DULCIESBOLOS.Models;

namespace DULCIESBOLOS.Data
{
    public static class DbInitializer
    {
        public static async Task SeedRolesAndAdminAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<Usuario>>();

            // 1. Criar os Papéis (Roles) se não existirem
            string[] roles = { "Admin", "Cliente" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // 2. Criar o Usuário Administrador padrão se não existir
            string adminEmail = "admin@dulciesbolos.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                var newAdmin = new Usuario
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    NomeCompleto = "Administrador Geral",
                    Endereco = "Loja Dulcies Bolos",
                    PhoneNumber = "11999999999",
                    EmailConfirmed = true
                };

                // Cria o usuário com a senha inicial segura
                var result = await userManager.CreateAsync(newAdmin, "Admin@123");

                if (result.Succeeded)
                {
                    // Associa ao perfil de Administrador
                    await userManager.AddToRoleAsync(newAdmin, "Admin");
                }
            }
        }
    }
}

