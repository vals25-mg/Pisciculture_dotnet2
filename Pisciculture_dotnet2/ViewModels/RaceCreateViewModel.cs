using System.ComponentModel.DataAnnotations;

namespace Pisciculture_dotnet2.ViewModels
{
    public class RaceCreateViewModel
    {
        [Required(ErrorMessage = "Le nom est requis")]
        [Display(Name = "Nom de la race")]
        public string NomRace { get; set; } = null!;
        
        [Required(ErrorMessage = "Le prix d'achat est requis")]
        [Range(0, double.MaxValue, ErrorMessage = "Le prix doit être positif")]
        [Display(Name = "Prix Achat Kg (Ar)")]
        public double PrixAchatKg { get; set; }
        
        [Required(ErrorMessage = "Le prix de vente est requis")]
        [Range(0, double.MaxValue, ErrorMessage = "Le prix doit être positif")]
        [Display(Name = "Prix Vente Kg (Ar)")]
        public double PrixVenteKg { get; set; }
        
        [Range(0, double.MaxValue, ErrorMessage = "Le poids doit être positif")]
        [Display(Name = "Poids Max (kg)")]
        public double? PoidsMax { get; set; }
        
        [Required(ErrorMessage = "L'apport en protéines est requis")]
        [Range(0, double.MaxValue, ErrorMessage = "L'apport doit être positif")]
        [Display(Name = "Besoin Protéine (g)")]
        public double ApportProteineG { get; set; }
        
        [Required(ErrorMessage = "L'apport en glucides est requis")]
        [Range(0, double.MaxValue, ErrorMessage = "L'apport doit être positif")]
        [Display(Name = "Besoin Glucide (g)")]
        public double ApportGlucideG { get; set; }
        
        [Required(ErrorMessage = "Le poids obtenu est requis")]
        [Range(0, double.MaxValue, ErrorMessage = "Le poids doit être positif")]
        [Display(Name = "Poids Obtenu (g)")]
        public double PoidsObtenuG { get; set; }
    }
}