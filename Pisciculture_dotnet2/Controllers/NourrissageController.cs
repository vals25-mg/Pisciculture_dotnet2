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
    public class NourrissageController : Controller
    {
        private readonly PiscicultureDbContext _context;

        public NourrissageController(PiscicultureDbContext context)
        {
            _context = context;
        }

        // GET: Nourrissage
        public async Task<IActionResult> Index()
        {
            var piscicultureDbContext = _context.Nourrissages
                .Include(n => n.IdAlimentNavigation)
                .Include(n => n.IdDoboNavigation)
                .OrderByDescending(n=>n.IdNourrissage);
            return View(await piscicultureDbContext.ToListAsync());
        }

        // GET: Nourrissage/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nourrissage = await _context.Nourrissages
                .Include(n => n.IdAlimentNavigation)
                .Include(n => n.IdDoboNavigation)
                .FirstOrDefaultAsync(m => m.IdNourrissage == id);
            if (nourrissage == null)
            {
                return NotFound();
            }

            return View(nourrissage);
        }

        // GET: Nourrissage/Create
        public IActionResult Create()
        {
            ViewData["IdAliment"] = new SelectList(_context.Aliments, "IdAliment", "NomAliment");
            ViewData["IdDobo"] = new SelectList(_context.Dobos, "IdDobo", "IdDobo");
            return View();
        }

        // POST: Nourrissage/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("IdNourrissage,IdAliment,IdDobo,PoidsAliments,DateNourrissage")] Nourrissage nourrissage)
        {
            if (ModelState.IsValid)
            {
                NourrissageUtilities.nourrirPoissons(_context,nourrissage);
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdAliment"] = new SelectList(_context.Aliments, "IdAliment", "NomAliment", nourrissage.IdAliment);
            ViewData["IdDobo"] = new SelectList(_context.Dobos, "IdDobo", "IdDobo", nourrissage.IdDobo);
            return View(nourrissage);
        }

        // GET: Nourrissage/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nourrissage = await _context.Nourrissages.FindAsync(id);
            if (nourrissage == null)
            {
                return NotFound();
            }
            ViewData["IdAliment"] = new SelectList(_context.Aliments, "IdAliment", "IdAliment", nourrissage.IdAliment);
            ViewData["IdDobo"] = new SelectList(_context.Dobos, "IdDobo", "IdDobo", nourrissage.IdDobo);
            return View(nourrissage);
        }

        // POST: Nourrissage/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdNourrissage,IdAliment,IdDobo,PoidsAliments,DateNourrissage")] Nourrissage nourrissage)
        {
            if (id != nourrissage.IdNourrissage)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(nourrissage);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NourrissageExists(nourrissage.IdNourrissage))
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
            ViewData["IdAliment"] = new SelectList(_context.Aliments, "IdAliment", "IdAliment", nourrissage.IdAliment);
            ViewData["IdDobo"] = new SelectList(_context.Dobos, "IdDobo", "IdDobo", nourrissage.IdDobo);
            return View(nourrissage);
        }

        // GET: Nourrissage/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nourrissage = await _context.Nourrissages
                .Include(n => n.IdAlimentNavigation)
                .Include(n => n.IdDoboNavigation)
                .FirstOrDefaultAsync(m => m.IdNourrissage == id);
            if (nourrissage == null)
            {
                return NotFound();
            }

            return View(nourrissage);
        }

        // POST: Nourrissage/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var nourrissage = await _context.Nourrissages.FindAsync(id);
            if (nourrissage != null)
            {
                _context.Nourrissages.Remove(nourrissage);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NourrissageExists(int id)
        {
            return _context.Nourrissages.Any(e => e.IdNourrissage == id);
        }
    }
}
