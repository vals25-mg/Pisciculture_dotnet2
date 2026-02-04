using System;
using System.Collections.Generic;

namespace Pisciculture_dotnet2.Models;

public partial class Nourrissage
{
    public int IdNourrissage { get; set; }

    public int? IdAliment { get; set; }

    public string? IdDobo { get; set; }

    public double? PoidsAliments { get; set; }

    public DateOnly? DateNourrissage { get; set; }

    public virtual Aliment? IdAlimentNavigation { get; set; }

    public virtual Dobo? IdDoboNavigation { get; set; }
}
