namespace Pisciculture_dotnet2.Models;

public class PoissonPoidsInfo
{
    public int IdRace { get; set; }
    public string NomRace { get; set; }
    public string IdBobo { get; set; }
    public string IdPoissonDobo { get; set; }
    public double PoidsTotalRecuKg { get; set; }
    public double PoidsInitialKg { get; set; }
    public double PoidsMaxKg { get; set; }
    public double PrixAchatKg { get; set; }

    public double PrixVenteKg { get; set; }
    public double PoidsActuelleKg => (PoidsInitialKg + PoidsTotalRecuKg);
    public double PoidsRestantKg => PoidsMaxKg - PoidsActuelleKg;
    public double ChiffreDaffaires => PoidsActuelleKg*PrixVenteKg;
    public double ChiffreDaffairesMoinsPrixAchat => ChiffreDaffaires-PrixAchatKg*PoidsActuelleKg;
}