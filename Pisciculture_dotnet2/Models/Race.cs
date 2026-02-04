using System;
using System.Collections.Generic;

namespace Pisciculture_dotnet2.Models;

public partial class Race
{
    public int IdRace { get; set; }

    public string NomRace { get; set; } = null!;

    public double PrixAchatKg { get; set; }

    public double PrixVenteKg { get; set; }

    public double? PoidsMax { get; set; }

    public virtual ICollection<CroissanceRace> CroissanceRaces { get; set; } = new List<CroissanceRace>();

    public virtual ICollection<EntreVague> EntreVagues { get; set; } = new List<EntreVague>();

    public virtual ICollection<PoissonDobo> PoissonDobos { get; set; } = new List<PoissonDobo>();
}
