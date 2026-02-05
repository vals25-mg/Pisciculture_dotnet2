using Microsoft.EntityFrameworkCore;
using Pisciculture_dotnet2.Models;

namespace Pisciculture_dotnet2.Utilities;

public static class NourrissageUtilities
{
    public static void insertNourrissage(
        PiscicultureDbContext dbContext, Nourrissage nourrissage)
    {
        if (nourrissage.PoidsAliments <= 0)
        {
            throw new Exception("Le poids doit être une valeur positive.");
        }
        dbContext.Add(nourrissage);
    }

    public static void nourrirPoissons(PiscicultureDbContext dbContext, Nourrissage nourrissage)
    {
        using (var transaction = dbContext.Database.BeginTransaction())
        {
            try
            {
                insertNourrissage(dbContext, nourrissage);
                Aliment aliment = AlimentUtilities.getAlimentById(dbContext, nourrissage.IdAliment.Value);
                double proteine_aliment_kg =
                    AlimentUtilities.getValueByPourcentage(nourrissage.PoidsAliments.Value,
                        aliment.PourcentageProteine);
                double glucide_aliment_kg =
                    AlimentUtilities.getValueByPourcentage(nourrissage.PoidsAliments.Value, aliment.PourcentageGlucide);

                // Liste trondro mbola tsy max ny poids
                var poissons = PoissonDoboUtilities.getPoissonsPoidsMaxNonAtteints(dbContext,nourrissage.IdDobo,nourrissage.DateNourrissage.Value);
                proteine_aliment_kg /= poissons.Count;
                glucide_aliment_kg /= poissons.Count;
                foreach (var p in poissons)
                {
                    CroissanceRace croissanceRace = CroissanceRaceUtilities.getByIdRace(dbContext,p.IdRace);
                    double apportParNutrimentKg = (croissanceRace.PoidsObtenuG.Value / 2)/1000;

                    double kgRecuProteine = getKgObtenuNutriment
                        (apportParNutrimentKg,proteine_aliment_kg,croissanceRace.ApportProteineG.Value);
                    double kgRecuGlucide = getKgObtenuNutriment
                        (apportParNutrimentKg,glucide_aliment_kg,croissanceRace.ApportGlucideG.Value);
                    double poidsTotalObtenu = kgRecuProteine + kgRecuGlucide;
                    
                    // Condition pour poids max atteint
                    if (poidsTotalObtenu + p.PoidsActuelleKg > p.PoidsMaxKg)
                    {
                        poidsTotalObtenu = p.PoidsRestantKg;
                    }
                    CroissancePoissonDoboUtilities.insertCroissancePoissonDobo
                        (dbContext,p.IdPoissonDobo,poidsTotalObtenu,nourrissage.DateNourrissage.Value);
                }
                dbContext.SaveChanges();
                transaction.Commit();
            }
            catch (Exception e)
            {
                transaction.Rollback();
                Console.WriteLine($"Erreur lors du nourrissage: {e.Message}");
                throw;
            }
        }
        
    }
    
    public static List<ResultatNourrissage> GetNourrissagePrixParDobo(PiscicultureDbContext dbContext, DateOnly? dateNourrissage = null)
    {
        var query = dbContext.VNourissagePrixAliments.AsQueryable();
    
        // Filtrer par date si fournie
        if (dateNourrissage.HasValue)
        {
            query = query.Where(x => x.DateNourrissage.Value <= dateNourrissage.Value);
        }
    
        // Grouper par date et dobo
        var sousRequete = query
            .GroupBy(x => new { x.DateNourrissage, x.IdDobo })
            .Select(g => new
            {
                DateNourrissage = g.Key.DateNourrissage,
                IdDobo = g.Key.IdDobo,
                PrixTotal = g.Sum(x => x.PrixTotal)
            })
            .ToList(); // Matérialiser en mémoire
    
        // Grouper par dobo et sommer avec ROW_NUMBER
        var resultat = sousRequete
            .GroupBy(x => x.IdDobo)
            .Select(g => new
            {
                IdDobo = g.Key,
                PrixTotal = g.Sum(x => x.PrixTotal)
            })
            .Select((item, index) => new ResultatNourrissage
            {
                Id = index + 1,
                IdDobo = item.IdDobo,
                PrixTotal = item.PrixTotal.Value
            })
            .ToList();
    
        return resultat;
    }

    public static double getKgObtenuNutriment(
        double apportParNutrimentKg,double nutriment_aliment_kg,double apport_proteine_g)
    {
        if(apport_proteine_g==0)
        return 0;
        
        return apportParNutrimentKg * nutriment_aliment_kg /
               (apport_proteine_g / 1000);
    }
}