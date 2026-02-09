using System.ComponentModel.DataAnnotations;

namespace Pisciculture_dotnet2.ViewModels
{
    public class RaceEditViewModel
    {
        public int IdRace { get; set; }
        
        [Required(ErrorMessage = "Le nom est requis")]
        public string NomRace { get; set; } = null!;
        
        [Required(ErrorMessage = "Le prix d'achat est requis")]
        [Range(0, double.MaxValue, ErrorMessage = "Le prix doit être positif")]
        public double PrixAchatKg { get; set; }
        
        [Required(ErrorMessage = "Le prix de vente est requis")]
        [Range(0, double.MaxValue, ErrorMessage = "Le prix doit être positif")]
        public double PrixVenteKg { get; set; }
        
        [Range(0, double.MaxValue, ErrorMessage = "Le poids doit être positif")]
        public double? PoidsMax { get; set; }
        
        [Required(ErrorMessage = "L'apport en protéines est requis")]
        [Range(0, double.MaxValue, ErrorMessage = "L'apport doit être positif")]
        public double ApportProteineG { get; set; }
        
        [Required(ErrorMessage = "L'apport en glucides est requis")]
        [Range(0, double.MaxValue, ErrorMessage = "L'apport doit être positif")]
        public double ApportGlucideG { get; set; }
        
        [Required(ErrorMessage = "Le poids obtenu est requis")]
        [Range(0, double.MaxValue, ErrorMessage = "Le poids doit être positif")]
        public double PoidsObtenuG { get; set; }
    }
}