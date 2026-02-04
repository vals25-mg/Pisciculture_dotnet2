using System;
using System.Collections.Generic;

namespace Pisciculture_dotnet2.Models;

public partial class PoissonDobo
{
    public string IdPoissonDobo { get; set; } = null!;

    public int? IdEntreVague { get; set; }

    public int? IdRace { get; set; }

    public double? PoidsInitialePoisson { get; set; }

    public virtual ICollection<CroissancePoissonDobo> CroissancePoissonDobos { get; set; } = new List<CroissancePoissonDobo>();

    public virtual EntreVague? IdEntreVagueNavigation { get; set; }

    public virtual Race? IdRaceNavigation { get; set; }
}
