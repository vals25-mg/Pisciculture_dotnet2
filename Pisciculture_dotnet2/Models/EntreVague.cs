using System;
using System.Collections.Generic;

namespace Pisciculture_dotnet2.Models;

public partial class EntreVague
{
    public int IdEntreVague { get; set; }

    public string? IdDobo { get; set; }

    public DateOnly? DateEntree { get; set; }

    public int? IdRace { get; set; }

    public int? NombrePoissons { get; set; }

    public double? PoidsInitialePoisson { get; set; }

    public virtual Dobo? IdDoboNavigation { get; set; }

    public virtual Race? IdRaceNavigation { get; set; }

    public virtual ICollection<PoissonDobo> PoissonDobos { get; set; } = new List<PoissonDobo>();
}
