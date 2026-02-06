using Microsoft.EntityFrameworkCore;
using Pisciculture_dotnet2.Models;

namespace Pisciculture_dotnet2.Utilities;

public class DoboUtilities
{
    public static void ReinitialiserDobo(PiscicultureDbContext dbContext, string id_dobo)
    {
        using (var transaction = dbContext.Database.BeginTransaction())
        {
            try
            {
                int[] id_entre_vagues = dbContext.EntreVagues
                    .Where(e => e.IdDobo == id_dobo)
                    .Select(e=>e.IdEntreVague)
                    .ToArray();
                foreach (int id in id_entre_vagues)
                {
                var idsPoissonDobos = dbContext.PoissonDobos
                    .Where(p => p.IdEntreVague == id)
                    .Select(p => p.IdPoissonDobo)
                    .ToList();

                dbContext.Nourrissages
                    .Where(n => n.IdDobo == id_dobo)
                    .ExecuteDelete();
                
                dbContext.CroissancePoissonDobos
                    .Where(c => idsPoissonDobos.Contains(c.IdPoissonDobo))
                    .ExecuteDelete();
                
                dbContext.PoissonDobos
                    .Where(p => p.IdEntreVague == id)
                    .ExecuteDelete();
                dbContext.EntreVagues
                    .Where(e => e.IdEntreVague == id)
                    .ExecuteDelete();
                    
                }
                
                dbContext.SaveChanges();
                transaction.Commit();
            }
            catch (Exception e)
            {
                transaction.Rollback();
                Console.WriteLine($"Erreur lors de la réinitialisation du dobo: {e.Message}");
                throw;
            }
        }
    }
}