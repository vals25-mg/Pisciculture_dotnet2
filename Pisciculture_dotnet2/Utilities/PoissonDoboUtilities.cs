using Microsoft.EntityFrameworkCore;
using Pisciculture_dotnet2.Models;

namespace Pisciculture_dotnet2.Utilities;

public static class PoissonDoboUtilities
{
    public static List<PoissonDobo> getPoissonsByIdDobo(PiscicultureDbContext dbContext, string id_dobo)
    {
        return dbContext.PoissonDobos
            .Include(p => p.CroissancePoissonDobos)  // Charger les croissances
            .Include(p => p.IdRaceNavigation)
            .Join(
                dbContext.EntreVagues.Where(e => e.IdDobo == id_dobo),
                p => p.IdEntreVague,
                e => e.IdEntreVague,
                (p, e) => p
            )
            .ToList();
    }
    
    public static List<PoissonPoidsInfo> getPoissonsPoidsMaxNonAtteints(PiscicultureDbContext dbContext, string id_dobo, DateOnly dateFiltre)
    {
        var poissonsExistants = getPoissonsByIdDobo(dbContext, id_dobo);
        return poissonsExistants.Select(
            p=> new PoissonPoidsInfo
            {
                IdRace = p.IdRace.Value,
                IdBobo = id_dobo,
                NomRace = p.IdRaceNavigation.NomRace,
                IdPoissonDobo = p.IdPoissonDobo,
                PoidsTotalRecuKg  = p.CroissancePoissonDobos
                    .Where(c => c.DateCroissance <= dateFiltre)
                    .Sum(c => c.PoidsRecuKg ?? 0),
                PoidsMaxKg = p.IdRaceNavigation?.PoidsMax ?? 0,
                PrixAchatKg = p.IdRaceNavigation?.PrixAchatKg ?? 0,
                PrixVenteKg = p.IdRaceNavigation?.PrixVenteKg ?? 0,
                PoidsInitialKg = p?.PoidsInitialePoisson ?? 0
            }
            )
            .Where(p=>p.PoidsActuelleKg<p.PoidsMaxKg)
            .ToList();
    }
    
    public static List<PoissonPoidsInfo> getAllPoissonsPoidsByIdDobo(PiscicultureDbContext dbContext, string id_dobo, DateOnly? dateFiltre = null)
    {
        var poissonsExistants = getPoissonsByIdDobo(dbContext, id_dobo);
        return poissonsExistants.Select(
                p=> new PoissonPoidsInfo
                {
                    IdRace = p.IdRace.Value,
                    IdBobo = id_dobo,
                    NomRace = p.IdRaceNavigation.NomRace,
                    IdPoissonDobo = p.IdPoissonDobo,
                    PoidsTotalRecuKg  = p.CroissancePoissonDobos
                        .Where(c => !dateFiltre.HasValue || c.DateCroissance <= dateFiltre.Value)
                        .Sum(c => c.PoidsRecuKg ?? 0),
                    PoidsMaxKg = p.IdRaceNavigation?.PoidsMax ?? 0,
                    PrixAchatKg = p.IdRaceNavigation?.PrixAchatKg ?? 0,
                    PrixVenteKg = p.IdRaceNavigation?.PrixVenteKg ?? 0,
                    PoidsInitialKg = p?.PoidsInitialePoisson ?? 0
                }
            )
            .ToList();
    }

    public static List<DoboInfos> getChiffresAffaires(PiscicultureDbContext dbContext, DateOnly? dateFiltre = null)
    {
        string[] id_dobos = dbContext.Dobos.Select(d => d.IdDobo).ToArray();
        
        var allPoissonPoidsInfo = id_dobos
            .SelectMany(id => getAllPoissonsPoidsByIdDobo(dbContext, id, dateFiltre))
            .ToList();
        
        var results = allPoissonPoidsInfo
            .GroupBy(p => p.IdBobo)
            .Select(g => new DoboInfos
            {
                IdDobo = g.Key,
                ChiffreDaffaires = g.Sum(p => p.ChiffreDaffaires),
                ChiffreDaffairesMoinsPrixAchat = g.Sum(p => p.ChiffreDaffairesMoinsPrixAchat),
                TotalPrixAchat = g.Sum(p=> p.PrixAchatKg*p.PoidsActuelleKg)
            })
            .ToList();
        return results;
    }

    public static double getBenefice(PiscicultureDbContext dbContext, DateOnly? dateFiltre = null)
    {
        var doboInfos = getChiffresAffaires(dbContext, dateFiltre);
        var depensesNourrissageDobo = NourrissageUtilities.GetNourrissagePrixParDobo(dbContext, dateFiltre);
    
        // Left Join pour inclure tous les dobos même sans nourrissage
        var beneficeTotal = doboInfos
            .GroupJoin(
                depensesNourrissageDobo,
                dobo => dobo.IdDobo,
                depense => depense.IdDobo,
                (dobo, depenses) => new
                {
                    dobo.IdDobo,
                    dobo.ChiffreDaffaires,
                    dobo.TotalPrixAchat,
                    PrixTotal = depenses.FirstOrDefault()?.PrixTotal ?? 0
                }
            )
            .Sum(x => x.ChiffreDaffaires - x.TotalPrixAchat - x.PrixTotal);
    
        return beneficeTotal;
    }
}