using System;
using System.Collections.Generic;

namespace Pisciculture_dotnet2.Models;

public partial class Dobo
{
    public string IdDobo { get; set; } = null!;

    public virtual ICollection<EntreVague> EntreVagues { get; set; } = new List<EntreVague>();

    public virtual ICollection<Nourrissage> Nourrissages { get; set; } = new List<Nourrissage>();
}
