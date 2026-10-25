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
    public class BolosController : Controller
    {
        private readonly AppDBContext _context;

        public BolosController(AppDBContext context)
        {
            _context = context;
        }

        // GET: Bolos
        public async Task<IActionResult> Index()
        {
            return View(await _context.Bolo.ToListAsync());
        }

        // GET: Bolos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bolo = await _context.Bolo
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bolo == null)
            {
                return NotFound();
            }

            return View(bolo);
        }

        // GET: Bolos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Bolos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Descricao,Tamanho,Valor")] Bolo bolo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(bolo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(bolo);
        }

        // GET: Bolos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bolo = await _context.Bolo.FindAsync(id);
            if (bolo == null)
            {
                return NotFound();
            }
            return View(bolo);
        }

        // POST: Bolos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Descricao,Tamanho,Valor")] Bolo bolo)
        {
            if (id != bolo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bolo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BoloExists(bolo.Id))
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
            return View(bolo);
        }

        // GET: Bolos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bolo = await _context.Bolo
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bolo == null)
            {
                return NotFound();
            }

            return View(bolo);
        }

        // POST: Bolos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bolo = await _context.Bolo.FindAsync(id);
            if (bolo != null)
            {
                _context.Bolo.Remove(bolo);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BoloExists(int id)
        {
            return _context.Bolo.Any(e => e.Id == id);
        }
    }
}
