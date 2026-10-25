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
    public class AcrescimosController : Controller
    {
        private readonly AppDBContext _context;

        public AcrescimosController(AppDBContext context)
        {
            _context = context;
        }

        // GET: Acrescimos
        public async Task<IActionResult> Index()
        {
            return View(await _context.Acrescimo.ToListAsync());
        }

        // GET: Acrescimos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var acrescimo = await _context.Acrescimo
                .FirstOrDefaultAsync(m => m.Id == id);
            if (acrescimo == null)
            {
                return NotFound();
            }

            return View(acrescimo);
        }

        // GET: Acrescimos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Acrescimos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Descricao,Quantidade,Valor")] Acrescimo acrescimo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(acrescimo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(acrescimo);
        }

        // GET: Acrescimos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var acrescimo = await _context.Acrescimo.FindAsync(id);
            if (acrescimo == null)
            {
                return NotFound();
            }
            return View(acrescimo);
        }

        // POST: Acrescimos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Descricao,Quantidade,Valor")] Acrescimo acrescimo)
        {
            if (id != acrescimo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(acrescimo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AcrescimoExists(acrescimo.Id))
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
            return View(acrescimo);
        }

        // GET: Acrescimos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var acrescimo = await _context.Acrescimo
                .FirstOrDefaultAsync(m => m.Id == id);
            if (acrescimo == null)
            {
                return NotFound();
            }

            return View(acrescimo);
        }

        // POST: Acrescimos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var acrescimo = await _context.Acrescimo.FindAsync(id);
            if (acrescimo != null)
            {
                _context.Acrescimo.Remove(acrescimo);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AcrescimoExists(int id)
        {
            return _context.Acrescimo.Any(e => e.Id == id);
        }
    }
}
