using System;
using System.Collections.Generic;

namespace Pisciculture_dotnet2.Models;

public partial class Aliment
{
    public int IdAliment { get; set; }

    public string NomAliment { get; set; } = null!;

    public double PourcentageProteine { get; set; }

    public double PourcentageGlucide { get; set; }

    public double PrixAchatKg { get; set; }

    public virtual ICollection<Nourrissage> Nourrissages { get; set; } = new List<Nourrissage>();
}
