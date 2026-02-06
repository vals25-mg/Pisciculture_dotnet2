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
    public class EntreVagueController : Controller
    {
        private readonly PiscicultureDbContext _context;

        public EntreVagueController(PiscicultureDbContext context)
        {
            _context = context;
        }

        // GET: EntreVague
        public async Task<IActionResult> Index()
        {
            var piscicultureDbContext = _context.EntreVagues.Include(e => e.IdDoboNavigation).Include(e => e.IdRaceNavigation);
            return View(await piscicultureDbContext.ToListAsync());
        }

        // GET: EntreVague/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entreVague = await _context.EntreVagues
                .Include(e => e.IdDoboNavigation)
                .Include(e => e.IdRaceNavigation)
                .FirstOrDefaultAsync(m => m.IdEntreVague == id);
            if (entreVague == null)
            {
                return NotFound();
            }

            return View(entreVague);
        }

        // GET: EntreVague/Create
        public IActionResult Create()
        {
            ViewData["IdDobo"] = new SelectList(_context.Dobos, "IdDobo", "IdDobo");
            ViewData["IdRace"] = new SelectList(_context.Races, "IdRace", "NomRace");
            return View();
        }

        // POST: EntreVague/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdEntreVague,IdDobo,DateEntree,IdRace,NombrePoissons,PoidsInitialePoisson")] EntreVague entreVague)
        {
            if (ModelState.IsValid)
            {
                _context.Add(entreVague);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdDobo"] = new SelectList(_context.Dobos, "IdDobo", "IdDobo", entreVague.IdDobo);
            ViewData["IdRace"] = new SelectList(_context.Races, "IdRace", "IdRace", entreVague.IdRace);
            return View(entreVague);
        }

        // GET: EntreVague/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entreVague = await _context.EntreVagues.FindAsync(id);
            if (entreVague == null)
            {
                return NotFound();
            }
            ViewData["IdDobo"] = new SelectList(_context.Dobos, "IdDobo", "IdDobo", entreVague.IdDobo);
            ViewData["IdRace"] = new SelectList(_context.Races, "IdRace", "IdRace", entreVague.IdRace);
            return View(entreVague);
        }

        // POST: EntreVague/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdEntreVague,IdDobo,DateEntree,IdRace,NombrePoissons,PoidsInitialePoisson")] EntreVague entreVague)
        {
            if (id != entreVague.IdEntreVague)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(entreVague);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EntreVagueExists(entreVague.IdEntreVague))
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
            ViewData["IdDobo"] = new SelectList(_context.Dobos, "IdDobo", "IdDobo", entreVague.IdDobo);
            ViewData["IdRace"] = new SelectList(_context.Races, "IdRace", "IdRace", entreVague.IdRace);
            return View(entreVague);
        }

        // GET: EntreVague/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entreVague = await _context.EntreVagues
                .Include(e => e.IdDoboNavigation)
                .Include(e => e.IdRaceNavigation)
                .FirstOrDefaultAsync(m => m.IdEntreVague == id);
            if (entreVague == null)
            {
                return NotFound();
            }

            return View(entreVague);
        }

        // POST: EntreVague/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var entreVague = await _context.EntreVagues.FindAsync(id);
            if (entreVague != null)
            {
                _context.EntreVagues.Remove(entreVague);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EntreVagueExists(int id)
        {
            return _context.EntreVagues.Any(e => e.IdEntreVague == id);
        }
    }
}
