using DULCIESBOLOS.Data;
using DULCIESBOLOS.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DULCIESBOLOS.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AgrupaFormaPagController : Controller
    {
        private readonly AppDBContext _context;
        //injeçao de dependencia do metodo do contexto
        public AgrupaFormaPagController(AppDBContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var lista = await _context.Venda
                //join com tabela de pagamento só pra pegar a descriçao
                .Include(v => v.Pagamento)
                .GroupBy(v => v.formaPagamentoId)
                .Select(g => new AgrupaFormaPag
                {
                    FormaPagamento = g.First().Pagamento.descricao,
                    NumeroVendas = g.Count(),
                    TotalArrecadado = g.Sum(v => v.Valor)
                })
                .ToListAsync();

            return View(lista);
        }
    }
}