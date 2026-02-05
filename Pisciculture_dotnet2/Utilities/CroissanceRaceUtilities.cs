using Pisciculture_dotnet2.Models;

namespace Pisciculture_dotnet2.Utilities;

public static class CroissanceRaceUtilities
{
    public static CroissanceRace getByIdRace(PiscicultureDbContext dbContext,int id_race)
    {
        return dbContext.CroissanceRaces.First(c => c.IdRace == id_race);
    }
}