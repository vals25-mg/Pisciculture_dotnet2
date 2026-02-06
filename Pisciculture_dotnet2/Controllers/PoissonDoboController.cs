using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Pisciculture_dotnet2.Models;

namespace Pisciculture_dotnet2.Controllers
{
    public class PoissonDoboController : Controller
    {
        private readonly PiscicultureDbContext _context;

        public PoissonDoboController(PiscicultureDbContext context)
        {
            _context = context;
        }

        // GET: PoissonDobo
        public async Task<IActionResult> Index()
        {
            var piscicultureDbContext = _context.PoissonDobos
                .Include(p => p.IdEntreVagueNavigation)
                .Include(p => p.IdRaceNavigation)
                .OrderBy(p=>p.IdPoissonDobo);
            return View(await piscicultureDbContext.ToListAsync());
        }

        // GET: PoissonDobo/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var poissonDobo = await _context.PoissonDobos
                .Include(p => p.IdEntreVagueNavigation)
                .Include(p => p.IdRaceNavigation)
                .FirstOrDefaultAsync(m => m.IdPoissonDobo == id);
            if (poissonDobo == null)
            {
                return NotFound();
            }

            return View(poissonDobo);
        }

        // GET: PoissonDobo/Create
        public IActionResult Create()
        {
            ViewData["IdEntreVague"] = new SelectList(_context.EntreVagues, "IdEntreVague", "IdEntreVague");
            ViewData["IdRace"] = new SelectList(_context.Races, "IdRace", "IdRace");
            return View();
        }

        // POST: PoissonDobo/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdPoissonDobo,IdEntreVague,IdRace,PoidsInitialePoisson")] PoissonDobo poissonDobo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(poissonDobo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdEntreVague"] = new SelectList(_context.EntreVagues, "IdEntreVague", "IdEntreVague", poissonDobo.IdEntreVague);
            ViewData["IdRace"] = new SelectList(_context.Races, "IdRace", "IdRace", poissonDobo.IdRace);
            return View(poissonDobo);
        }

        // GET: PoissonDobo/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var poissonDobo = await _context.PoissonDobos.FindAsync(id);
            if (poissonDobo == null)
            {
                return NotFound();
            }
            ViewData["IdEntreVague"] = new SelectList(_context.EntreVagues, "IdEntreVague", "IdEntreVague", poissonDobo.IdEntreVague);
            ViewData["IdRace"] = new SelectList(_context.Races, "IdRace", "NomRace", poissonDobo.IdRace);
            return View(poissonDobo);
        }

        // POST: PoissonDobo/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("IdPoissonDobo,IdEntreVague,IdRace,PoidsInitialePoisson")] PoissonDobo poissonDobo)
        {
            if (id != poissonDobo.IdPoissonDobo)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var poissonExistant = await _context.PoissonDobos.FindAsync(id);
            
                    if (poissonExistant == null)
                    {
                        return NotFound();
                    }
                    poissonExistant.IdRace = poissonDobo.IdRace;
                    poissonExistant.PoidsInitialePoisson = poissonDobo.PoidsInitialePoisson;

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PoissonDoboExists(poissonDobo.IdPoissonDobo))
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
            ViewData["IdEntreVague"] = new SelectList(_context.EntreVagues, "IdEntreVague", "IdEntreVague", poissonDobo.IdEntreVague);
            ViewData["IdRace"] = new SelectList(_context.Races, "IdRace", "IdRace", poissonDobo.IdRace);
            return View(poissonDobo);
        }

        // GET: PoissonDobo/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var poissonDobo = await _context.PoissonDobos
                .Include(p => p.IdEntreVagueNavigation)
                .Include(p => p.IdRaceNavigation)
                .FirstOrDefaultAsync(m => m.IdPoissonDobo == id);
            if (poissonDobo == null)
            {
                return NotFound();
            }

            return View(poissonDobo);
        }

        // POST: PoissonDobo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var poissonDobo = await _context.PoissonDobos.FindAsync(id);
            if (poissonDobo != null)
            {
                _context.PoissonDobos.Remove(poissonDobo);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PoissonDoboExists(string id)
        {
            return _context.PoissonDobos.Any(e => e.IdPoissonDobo == id);
        }
    }
}
