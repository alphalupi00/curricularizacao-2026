#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DULCIESBOLOS.Data;
using DULCIESBOLOS.Models;

namespace DULCIESBOLOS.Controllers
{
    public class VendasController : Controller
    {
        private readonly AppDBContext _context;

        public VendasController(AppDBContext context)
        {
            _context = context;
        }

        // GET: Vendas
        public async Task<IActionResult> Index()
        {
            var vendas = _context.Venda
                .Include(v => v.Bolo)
                .Include(v => v.Usuario)
                .Include(v => v.Pagamento)
                .OrderByDescending(v => v.DataVenda);

            return View(await vendas.ToListAsync());
        }

        // GET: Vendas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venda = await _context.Venda
                .Include(v => v.Bolo)
                .Include(v => v.Usuario)
                .Include(v => v.Pagamento)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (venda == null)
            {
                return NotFound();
            }

            return View(venda);
        }

        // GET: Vendas/Create
        public async Task<IActionResult> Create()
        {
            await CarregarListasDropdownAsync();
            return View();
        }

        // POST: Vendas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UsuarioId,boloId,Quantidade,formaPagamentoId")] Venda venda)
        {
            // Busca o bolo selecionado para calcular o valor total com base no preço e quantidade
            if (venda.boloId.HasValue)
            {
                var bolo = await _context.Bolo.FindAsync(venda.boloId.Value);
                if (bolo != null)
                {
                    venda.Valor = (decimal)bolo.Valor * venda.Quantidade;
                }
            }

            venda.DataVenda = DateTime.Now;
            venda.Status = "Pendente";

            // Remove validações automáticas de propriedades de navegação para não bloquear o cadastro
            ModelState.Remove("Usuario");
            ModelState.Remove("Bolo");
            ModelState.Remove("Pagamento");

            if (ModelState.IsValid)
            {
                _context.Add(venda);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            await CarregarListasDropdownAsync(venda.boloId, venda.UsuarioId, venda.formaPagamentoId);
            return View(venda);
        }

        // GET: Vendas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venda = await _context.Venda.FindAsync(id);
            if (venda == null)
            {
                return NotFound();
            }

            await CarregarListasDropdownAsync(venda.boloId, venda.UsuarioId, venda.formaPagamentoId);
            return View(venda);
        }

        // POST: Vendas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UsuarioId,boloId,Quantidade,formaPagamentoId,Status,DataVenda")] Venda venda)
        {
            if (id != venda.Id)
            {
                return NotFound();
            }

            if (venda.boloId.HasValue)
            {
                var bolo = await _context.Bolo.FindAsync(venda.boloId.Value);
                if (bolo != null)
                {
                    venda.Valor = (decimal)bolo.Valor * venda.Quantidade;
                }
            }

            ModelState.Remove("Usuario");
            ModelState.Remove("Bolo");
            ModelState.Remove("Pagamento");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(venda);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VendaExists(venda.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            await CarregarListasDropdownAsync(venda.boloId, venda.UsuarioId, venda.formaPagamentoId);
            return View(venda);
        }

        // GET: Vendas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venda = await _context.Venda
                .Include(v => v.Bolo)
                .Include(v => v.Usuario)
                .Include(v => v.Pagamento)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (venda == null)
            {
                return NotFound();
            }

            return View(venda);
        }

        // POST: Vendas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var venda = await _context.Venda.FindAsync(id);
            if (venda != null)
            {
                _context.Venda.Remove(venda);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VendaExists(int id)
        {
            return _context.Venda.Any(e => e.Id == id);
        }

        /// <summary>
        /// Carrega os menus suspensos (dropdowns) com informações completas e amigáveis:
        /// exibe sabor, tamanho (P, M ou G) e o valor no menu de bolos.
        /// </summary>
        private async Task CarregarListasDropdownAsync(int? boloId = null, string? usuarioId = null, int? formaPagamentoId = null)
        {
            var bolos = await _context.Bolo
                .OrderBy(b => b.Descricao)
                .ThenBy(b => b.Tamanho)
                .ToListAsync();

            var listaBolos = bolos.Select(b => new
            {
                Id = b.Id,
                TextoExibicao = $"{b.Descricao} - Tamanho {b.Tamanho.ToUpper()} (R$ {b.Valor:N2})"
            }).ToList();

            ViewData["boloId"] = new SelectList(listaBolos, "Id", "TextoExibicao", boloId);

            var usuarios = await _context.Users
                .OrderBy(u => u.NomeCompleto)
                .ToListAsync();

            ViewData["UsuarioId"] = new SelectList(usuarios, "Id", "NomeCompleto", usuarioId);

            var pagamentos = await _context.Pagamento
                .OrderBy(p => p.descricao)
                .ToListAsync();

            ViewData["formaPagamentoId"] = new SelectList(pagamentos, "Id", "descricao", formaPagamentoId);
        }
    }
}
