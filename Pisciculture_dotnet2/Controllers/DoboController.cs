using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Pisciculture_dotnet2.Models;
using Pisciculture_dotnet2.Utilities;

namespace Pisciculture_dotnet2.Controllers
{
    public class DoboController : Controller
    {
        private readonly PiscicultureDbContext _context;

        public DoboController(PiscicultureDbContext context)
        {
            _context = context;
        }

        // GET: Dobo
        public async Task<IActionResult> Index()
        {
            return View(await _context.Dobos.ToListAsync());
        }

        // POST: Dobo/Reinitialiser/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reinitialiser(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            try
            {
                DoboUtilities.ReinitialiserDobo(_context, id);
                TempData["SuccessMessage"] = $"Le dobo {id} a été réinitialisé avec succès.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Erreur lors de la réinitialisation : {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }
        
        // GET: Dobo/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dobo = await _context.Dobos
                .FirstOrDefaultAsync(m => m.IdDobo == id);
            if (dobo == null)
            {
                return NotFound();
            }

            return View(dobo);
        }

        // GET: Dobo/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Dobo/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdDobo")] Dobo dobo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dobo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(dobo);
        }

        // GET: Dobo/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dobo = await _context.Dobos.FindAsync(id);
            if (dobo == null)
            {
                return NotFound();
            }
            return View(dobo);
        }

        // POST: Dobo/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("IdDobo")] Dobo dobo)
        {
            if (id != dobo.IdDobo)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dobo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DoboExists(dobo.IdDobo))
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
            return View(dobo);
        }

        // GET: Dobo/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dobo = await _context.Dobos
                .FirstOrDefaultAsync(m => m.IdDobo == id);
            if (dobo == null)
            {
                return NotFound();
            }

            return View(dobo);
        }

        // POST: Dobo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var dobo = await _context.Dobos.FindAsync(id);
            if (dobo != null)
            {
                _context.Dobos.Remove(dobo);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DoboExists(string id)
        {
            return _context.Dobos.Any(e => e.IdDobo == id);
        }
    }
}
