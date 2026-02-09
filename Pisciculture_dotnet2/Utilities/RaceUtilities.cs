using Microsoft.EntityFrameworkCore;
using Pisciculture_dotnet2.Models;

namespace Pisciculture_dotnet2.Utilities
{
    public class RaceUtilities
    {
        public static void UpdateRaceWithCroissance(
            PiscicultureDbContext dbContext, 
            Race race, 
            double apportProteineG, 
            double apportGlucideG, 
            double poidsObtenuG)
        {
            using (var transaction = dbContext.Database.BeginTransaction())
            {
                try
                {
                    // Récupérer la race existante
                    var raceExistante = dbContext.Races
                        .Include(r => r.CroissanceRaces)
                        .FirstOrDefault(r => r.IdRace == race.IdRace);

                    if (raceExistante == null)
                    {
                        throw new Exception("Race introuvable.");
                    }

                    // Mettre à jour les propriétés de la race
                    raceExistante.NomRace = race.NomRace;
                    raceExistante.PrixAchatKg = race.PrixAchatKg;
                    raceExistante.PrixVenteKg = race.PrixVenteKg;
                    raceExistante.PoidsMax = race.PoidsMax;

                    // Mettre à jour ou créer la croissance
                    var croissance = raceExistante.CroissanceRaces.FirstOrDefault();
                    
                    if (croissance != null)
                    {
                        // Mise à jour de la croissance existante
                        croissance.ApportProteineG = apportProteineG;
                        croissance.ApportGlucideG = apportGlucideG;
                        croissance.PoidsObtenuG = poidsObtenuG;
                    }
                    else
                    {
                        // Création d'une nouvelle croissance si elle n'existe pas
                        var nouvelleCroissance = new CroissanceRace
                        {
                            IdRace = race.IdRace,
                            ApportProteineG = apportProteineG,
                            ApportGlucideG = apportGlucideG,
                            PoidsObtenuG = poidsObtenuG,
                        };
                        dbContext.CroissanceRaces.Add(nouvelleCroissance);
                    }

                    dbContext.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    Console.WriteLine($"Erreur lors de la mise à jour : {e.Message}");
                    throw;
                }
            }
        }
        
        public static void CreateRaceWithCroissance(
            PiscicultureDbContext dbContext,
            Race race,
            double apportProteineG,
            double apportGlucideG,
            double poidsObtenuG)
        {
            using (var transaction = dbContext.Database.BeginTransaction())
            {
                try
                {
                    // Ajouter la race
                    dbContext.Races.Add(race);
                    dbContext.SaveChanges();

                    // Créer la croissance associée
                    var croissance = new CroissanceRace
                    {
                        IdRace = race.IdRace,
                        ApportProteineG = apportProteineG,
                        ApportGlucideG = apportGlucideG,
                        PoidsObtenuG = poidsObtenuG,
                    };

                    dbContext.CroissanceRaces.Add(croissance);
                    dbContext.SaveChanges();

                    transaction.Commit();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    Console.WriteLine($"Erreur lors de la création : {e.Message}");
                    throw;
                }
            }
        }
    }
}