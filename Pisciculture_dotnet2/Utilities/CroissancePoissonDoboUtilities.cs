using Pisciculture_dotnet2.Models;

namespace Pisciculture_dotnet2.Utilities;

public static class CroissancePoissonDoboUtilities
{
    public static void insertCroissancePoissonDobo(
        PiscicultureDbContext dbContext,string id_poisson_dobo,double poids_recu_kg, DateOnly dateNourrissage)
    {
        var croissancePoissonDobo = new CroissancePoissonDobo
        {
            IdPoissonDobo = id_poisson_dobo,
            PoidsRecuKg = poids_recu_kg,
            DateCroissance = dateNourrissage
        };
        dbContext.CroissancePoissonDobos.Add(croissancePoissonDobo);
        dbContext.SaveChanges();
    }
}