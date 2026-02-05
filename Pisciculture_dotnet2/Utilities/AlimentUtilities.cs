using Pisciculture_dotnet2.Models;

namespace Pisciculture_dotnet2.Utilities;

public static class AlimentUtilities
{
    public static Aliment getAlimentById(
        PiscicultureDbContext dbContext, int id_aliment)
    {
        var aliment = dbContext.Aliments.FirstOrDefault(a => a.IdAliment == id_aliment);
        if (aliment==null)
        {
            throw new Exception("Aliment introuvable!");
        }

        return aliment;
    }

    public static double getValueByPourcentage(double initialValue, double pourcentage)
    {
        return initialValue * pourcentage / 100;
    }
}