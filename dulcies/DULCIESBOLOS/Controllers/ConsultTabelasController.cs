using DULCIESBOLOS.Data;
using DULCIESBOLOS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DULCIESBOLOS.Controllers
{
    public class ConsultTabelasController : Controller
    {
        private readonly AppDBContext _context;

        public ConsultTabelasController(AppDBContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var consulta = await _context.Venda
                .Join(
                    _context.Bolo,
                    venda => venda.boloId,
                    bolo => (int?)bolo.Id,
                    (venda, bolo) => new { venda, bolo }
                )
                .GroupBy(x => x.bolo.Descricao)
                .Select(grupo => new ConsultTabelas
                {
                    Sabor = grupo.Key,

                    QuantidadeVendida = grupo.Sum(x => x.venda.Quantidade),

                    FaturamentoTotal = grupo.Sum(
                        item => (decimal)item.bolo.Valor * item.venda.Quantidade
                    )
                })
                .ToListAsync();

            return View(consulta);
        }
    }
}