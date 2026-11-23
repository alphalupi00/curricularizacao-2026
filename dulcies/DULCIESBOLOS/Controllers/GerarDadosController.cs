using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DULCIESBOLOS.Data;
using DULCIESBOLOS.Models;

namespace DULCIESBOLOS.Controllers
{
    public class GerarDadosController : Controller
    {
        private readonly AppDBContext contexto;

        public GerarDadosController(AppDBContext context)
        {
            contexto = context;
        }

        public IActionResult gerarAcrescimos()
        {
            contexto.Database.ExecuteSqlRaw("DELETE FROM Acrescimo");
            contexto.Database.ExecuteSqlRaw("DBCC CHECKIDENT('Acrescimo', RESEED, 0)");

            contexto.Acrescimo.Add(new Acrescimo { Descricao = "Confete", Quantidade = "5g", Valor = 2.50m });
            contexto.Acrescimo.Add(new Acrescimo { Descricao = "Paçoca", Quantidade = "5g", Valor = 3.50m });
            contexto.Acrescimo.Add(new Acrescimo { Descricao = "Caldo de Chocolate", Quantidade = "7g", Valor = 4.75m });
            contexto.Acrescimo.Add(new Acrescimo { Descricao = "Caldo de Morango", Quantidade = "7g", Valor = 4.75m });
            contexto.Acrescimo.Add(new Acrescimo { Descricao = "Caldo de Caramelo", Quantidade = "7g", Valor = 4.75m });

            contexto.SaveChanges();

            return View(contexto.Acrescimo.ToList());
        }

        public IActionResult gerarBolos()
        {
            contexto.Database.ExecuteSqlRaw("DELETE FROM Bolo");
            contexto.Database.ExecuteSqlRaw("DBCC CHECKIDENT('Bolo', RESEED, 0)");

            contexto.Bolo.Add(new Bolo { Descricao = "Bolo de Chocolate", Tamanho = "P", Valor = 20.00f });
            contexto.Bolo.Add(new Bolo { Descricao = "Bolo de Chocolate", Tamanho = "M", Valor = 25.00f });
            contexto.Bolo.Add(new Bolo { Descricao = "Bolo de Chocolate", Tamanho = "G", Valor = 30.00f });
            contexto.Bolo.Add(new Bolo { Descricao = "Bolo de Morango", Tamanho = "P", Valor = 20.00f });
            contexto.Bolo.Add(new Bolo { Descricao = "Bolo de Morango", Tamanho = "M", Valor = 25.00f });
            contexto.Bolo.Add(new Bolo { Descricao = "Bolo de Morango", Tamanho = "G", Valor = 30.00f });
            contexto.Bolo.Add(new Bolo { Descricao = "Bolo de Ninho", Tamanho = "P", Valor = 20.00f });
            contexto.Bolo.Add(new Bolo { Descricao = "Bolo de Ninho", Tamanho = "M", Valor = 25.00f });
            contexto.Bolo.Add(new Bolo { Descricao = "Bolo de Ninho", Tamanho = "G", Valor = 30.00f });

            contexto.SaveChanges();

            return View(contexto.Bolo.ToList());
        }

        public IActionResult gerarPagamentos()
        {
            contexto.Database.ExecuteSqlRaw("DELETE FROM Pagamento");
            contexto.Database.ExecuteSqlRaw("DBCC CHECKIDENT('Pagamento', RESEED, 0)");

            contexto.Pagamento.Add(new Pagamento { descricao = "Débito" });
            contexto.Pagamento.Add(new Pagamento { descricao = "Crédito" });
            contexto.Pagamento.Add(new Pagamento { descricao = "Dinheiro" });
            contexto.Pagamento.Add(new Pagamento { descricao = "Pix" });

            contexto.SaveChanges();

            return View(contexto.Pagamento.ToList());
        }

        public IActionResult gerarUsuarios()
        {
            contexto.Database.ExecuteSqlRaw("DELETE FROM Usuario");

            contexto.Users.Add(new Usuario { NomeCompleto = "Gusthavo Erick da Silva Serra", Endereco = "Rua ABCD 111" });
            contexto.Users.Add(new Usuario { NomeCompleto = "Ruan Gabriel Pelegrin da Silva", Endereco = "Rua ACBD 222" });
            contexto.Users.Add(new Usuario { NomeCompleto = "Sarah Delamura Candela", Endereco = "Rua ADBC 333" });
            contexto.Users.Add(new Usuario { NomeCompleto = "Sophia Oliveira Michellis", Endereco = "Rua BACD 444" });

            contexto.SaveChanges();

            return View(contexto.Users.ToList());
        }
    }
}