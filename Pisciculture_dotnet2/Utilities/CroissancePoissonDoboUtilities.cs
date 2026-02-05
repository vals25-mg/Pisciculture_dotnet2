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
    
    public static List<CroissancePoissonDobo> getCroissancesByIdPoissonDobo(
        PiscicultureDbContext dbContext, 
        string idPoissonDobo, 
        DateOnly? dateFiltre = null)
    {
        var query = dbContext.CroissancePoissonDobos
            .Where(c => c.IdPoissonDobo == idPoissonDobo);
    
        // Appliquer le filtre de date si fourni
        if (dateFiltre.HasValue)
        {
            query = query.Where(c => c.DateCroissance <= dateFiltre.Value);
        }
    
        // Trier par date de croissance (du plus ancien au plus récent)
        return query
            .OrderBy(c => c.DateCroissance)
            .ToList();
    }
}