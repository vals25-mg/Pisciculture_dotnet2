using System;
using System.Collections.Generic;

namespace Pisciculture_dotnet2.Models;

public partial class CroissancePoissonDobo
{
    public int IdCroissancePoissonDobo { get; set; }

    public string? IdPoissonDobo { get; set; }

    public double? PoidsRecuKg { get; set; }

    public DateOnly? DateCroissance { get; set; }

    public virtual PoissonDobo? IdPoissonDoboNavigation { get; set; }
}
