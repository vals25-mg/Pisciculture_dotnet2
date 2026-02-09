using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Pisciculture_dotnet2.Models;
using Pisciculture_dotnet2.Utilities;
using Pisciculture_dotnet2.ViewModels;

namespace Pisciculture_dotnet2.Controllers
{
    public class RaceController : Controller
    {
        private readonly PiscicultureDbContext _context;

        public RaceController(PiscicultureDbContext context)
        {
            _context = context;
        }

        // GET: Race
        public async Task<IActionResult> Index()
        {
            return View(await _context.Races
                .Include(r => r.CroissanceRaces)
                .ToListAsync());
        }

        // GET: Race/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var race = await _context.Races
                .FirstOrDefaultAsync(m => m.IdRace == id);
            if (race == null)
            {
                return NotFound();
            }

            return View(race);
        }

        // GET: Race/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Race/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RaceCreateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var race = new Race
                    {
                        NomRace = viewModel.NomRace,
                        PrixAchatKg = viewModel.PrixAchatKg,
                        PrixVenteKg = viewModel.PrixVenteKg,
                        PoidsMax = viewModel.PoidsMax
                    };

                    RaceUtilities.CreateRaceWithCroissance(
                        _context,
                        race,
                        viewModel.ApportProteineG,
                        viewModel.ApportGlucideG,
                        viewModel.PoidsObtenuG
                    );

                    TempData["SuccessMessageRace"] = "Race créée avec succès.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Erreur lors de la création : {ex.Message}");
                }
            }
            return View(viewModel);
        }
        // GET: Race/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var race = await _context.Races
                .Include(r => r.CroissanceRaces)
                .FirstOrDefaultAsync(r => r.IdRace == id);
        
            if (race == null)
            {
                return NotFound();
            }

            var croissance = race.CroissanceRaces.FirstOrDefault();
    
            var viewModel = new RaceEditViewModel
            {
                IdRace = race.IdRace,
                NomRace = race.NomRace,
                PrixAchatKg = race.PrixAchatKg,
                PrixVenteKg = race.PrixVenteKg,
                PoidsMax = race.PoidsMax,
                ApportProteineG = croissance?.ApportProteineG ?? 0,
                ApportGlucideG = croissance?.ApportGlucideG ?? 0,
                PoidsObtenuG = croissance?.PoidsObtenuG ?? 0
            };

            return View(viewModel);
        }


        // POST: Race/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, RaceEditViewModel viewModel)
        {
            if (id != viewModel.IdRace)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var race = new Race
                    {
                        IdRace = viewModel.IdRace,
                        NomRace = viewModel.NomRace,
                        PrixAchatKg = viewModel.PrixAchatKg,
                        PrixVenteKg = viewModel.PrixVenteKg,
                        PoidsMax = viewModel.PoidsMax
                    };

                    RaceUtilities.UpdateRaceWithCroissance(
                        _context,
                        race,
                        viewModel.ApportProteineG,
                        viewModel.ApportGlucideG,
                        viewModel.PoidsObtenuG
                    );

                    TempData["SuccessMessageRace"] = "Race modifiée avec succès.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Erreur lors de la modification : {ex.Message}");
                }
            }
    
            return View(viewModel);
        }
        // GET: Race/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var race = await _context.Races
                .FirstOrDefaultAsync(m => m.IdRace == id);
            if (race == null)
            {
                return NotFound();
            }

            return View(race);
        }

        // POST: Race/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var race = await _context.Races.FindAsync(id);
            if (race != null)
            {
                _context.Races.Remove(race);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RaceExists(int id)
        {
            return _context.Races.Any(e => e.IdRace == id);
        }
    }
}
