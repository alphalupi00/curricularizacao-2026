using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
            var context = serviceProvider.GetRequiredService<AppDBContext>();

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

            // 3. Criar o Usuário Cliente padrão se não existir
            string clienteEmail = "cliente@dulciesbolos.com";
            var clienteUser = await userManager.FindByEmailAsync(clienteEmail);

            if (clienteUser == null)
            {
                var newCliente = new Usuario
                {
                    UserName = clienteEmail,
                    Email = clienteEmail,
                    NomeCompleto = "Cliente Padrão",
                    Endereco = "Rua das Delícias, 123",
                    PhoneNumber = "11988888888",
                    EmailConfirmed = true
                };

                var resultCliente = await userManager.CreateAsync(newCliente, "Cliente@123");

                if (resultCliente.Succeeded)
                {
                    // Associa ao perfil de Cliente
                    await userManager.AddToRoleAsync(newCliente, "Cliente");
                }
            }

            // 4. Carga Inicial de Bolos (se a tabela estiver vazia)
            if (!await context.Bolo.AnyAsync())
            {
                var bolosIniciais = new[]
                {
                    new Bolo { Descricao = "Chocolate com cobertura de chocolate", Tamanho = "P", Valor = 35.0f },
                    new Bolo { Descricao = "Chocolate com cobertura de chocolate", Tamanho = "M", Valor = 55.0f },
                    new Bolo { Descricao = "Chocolate com cobertura de chocolate", Tamanho = "G", Valor = 75.0f },
                    new Bolo { Descricao = "Cenoura com cobertura de chocolate", Tamanho = "P", Valor = 30.0f },
                    new Bolo { Descricao = "Cenoura com cobertura de chocolate", Tamanho = "M", Valor = 45.0f },
                    new Bolo { Descricao = "Cenoura com cobertura de chocolate", Tamanho = "G", Valor = 65.0f },
                    new Bolo { Descricao = "Limão com cobertura de limão", Tamanho = "P", Valor = 30.0f },
                    new Bolo { Descricao = "Limão com cobertura de limão", Tamanho = "M", Valor = 45.0f },
                    new Bolo { Descricao = "Limão com cobertura de limão", Tamanho = "G", Valor = 65.0f },
                    new Bolo { Descricao = "Ninho e morango", Tamanho = "P", Valor = 40.0f },
                    new Bolo { Descricao = "Ninho e morango", Tamanho = "M", Valor = 65.0f },
                    new Bolo { Descricao = "Ninho e morango", Tamanho = "G", Valor = 85.0f },
                    new Bolo { Descricao = "Iogurte com cobertura de ninho", Tamanho = "P", Valor = 35.0f },
                    new Bolo { Descricao = "Iogurte com cobertura de ninho", Tamanho = "M", Valor = 50.0f },
                    new Bolo { Descricao = "Iogurte com cobertura de ninho", Tamanho = "G", Valor = 70.0f },
                    new Bolo { Descricao = "Brownie", Tamanho = "P", Valor = 25.0f },
                    new Bolo { Descricao = "Brownie", Tamanho = "M", Valor = 40.0f },
                    new Bolo { Descricao = "Brownie", Tamanho = "G", Valor = 60.0f }
                };

                await context.Bolo.AddRangeAsync(bolosIniciais);
                await context.SaveChangesAsync();
            }

            // 5. Carga Inicial de Acréscimos (se a tabela estiver vazia)
            if (!await context.Acrescimo.AnyAsync())
            {
                var acrescimosIniciais = new[]
                {
                    new Acrescimo { Descricao = "Morangos Frescos", Quantidade = "100g", Valor = 8.00m },
                    new Acrescimo { Descricao = "Cobertura Extra de Brigadeiro", Quantidade = "100g", Valor = 8.00m },
                    new Acrescimo { Descricao = "Cobertura Extra de Leite Ninho", Quantidade = "100g", Valor = 8.00m },
                    new Acrescimo { Descricao = "Nutella Pura", Quantidade = "100g", Valor = 12.00m },
                    new Acrescimo { Descricao = "Granulado Belga", Quantidade = "50g", Valor = 6.00m },
                    new Acrescimo { Descricao = "Calda de Frutas Vermelhas", Quantidade = "100g", Valor = 9.00m },
                    new Acrescimo { Descricao = "Raspas de Chocolate Nobre", Quantidade = "50g", Valor = 7.00m }
                };

                await context.Acrescimo.AddRangeAsync(acrescimosIniciais);
                await context.SaveChangesAsync();
            }
        }
    }
}

