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
            var appDBContext = _context.Venda.Include(v => v.Bolo).Include(v => v.Cliente).Include(v => v.Funcionario).Include(v => v.Pagamento);
            return View(await appDBContext.ToListAsync());
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
                .Include(v => v.Cliente)
                .Include(v => v.Funcionario)
                .Include(v => v.Pagamento)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (venda == null)
            {
                return NotFound();
            }

            return View(venda);
        }

        // GET: Vendas/Create
        public IActionResult Create()
        {
            ViewData["boloId"] = new SelectList(_context.Bolo, "Id", "Descricao");
            ViewData["clienteId"] = new SelectList(_context.Cliente, "Id", "Endereco");
            ViewData["funcionarioId"] = new SelectList(_context.Funcionarios, "Id", "Nome");
            ViewData["formaPagamentoId"] = new SelectList(_context.Pagamento, "Id", "descricao");
            return View();
        }

        // POST: Vendas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,clienteId,funcionarioId,boloId,Quantidade,Valor,formaPagamentoId")] Venda venda)
        {
            if (ModelState.IsValid)
            {
                _context.Add(venda);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["boloId"] = new SelectList(_context.Bolo, "Id", "Descricao", venda.boloId);
            ViewData["clienteId"] = new SelectList(_context.Cliente, "Id", "Endereco", venda.clienteId);
            ViewData["funcionarioId"] = new SelectList(_context.Funcionarios, "Id", "Nome", venda.funcionarioId);
            ViewData["formaPagamentoId"] = new SelectList(_context.Pagamento, "Id", "descricao", venda.formaPagamentoId);
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
            ViewData["boloId"] = new SelectList(_context.Bolo, "Id", "Descricao", venda.boloId);
            ViewData["clienteId"] = new SelectList(_context.Cliente, "Id", "Endereco", venda.clienteId);
            ViewData["funcionarioId"] = new SelectList(_context.Funcionarios, "Id", "Nome", venda.funcionarioId);
            ViewData["formaPagamentoId"] = new SelectList(_context.Pagamento, "Id", "descricao", venda.formaPagamentoId);
            return View(venda);
        }

        // POST: Vendas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,clienteId,funcionarioId,boloId,Quantidade,Valor,formaPagamentoId")] Venda venda)
        {
            if (id != venda.Id)
            {
                return NotFound();
            }

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
            ViewData["boloId"] = new SelectList(_context.Bolo, "Id", "Descricao", venda.boloId);
            ViewData["clienteId"] = new SelectList(_context.Cliente, "Id", "Endereco", venda.clienteId);
            ViewData["funcionarioId"] = new SelectList(_context.Funcionarios, "Id", "Nome", venda.funcionarioId);
            ViewData["formaPagamentoId"] = new SelectList(_context.Pagamento, "Id", "descricao", venda.formaPagamentoId);
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
                .Include(v => v.Cliente)
                .Include(v => v.Funcionario)
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
    }
}
