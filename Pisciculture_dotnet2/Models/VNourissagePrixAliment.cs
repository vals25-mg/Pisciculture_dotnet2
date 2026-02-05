using System;
using System.Collections.Generic;

namespace Pisciculture_dotnet2.Models;

public partial class VNourissagePrixAliment
{
    public long? Id { get; set; }

    public int? IdAliment { get; set; }

    public string? NomAliment { get; set; }

    public DateOnly? DateNourrissage { get; set; }

    public string? IdDobo { get; set; }

    public double? PoidsAliments { get; set; }

    public double? PrixAchatKg { get; set; }

    public double? PrixTotal { get; set; }
}
