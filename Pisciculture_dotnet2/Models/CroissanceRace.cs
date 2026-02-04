using System;
using System.Collections.Generic;

namespace Pisciculture_dotnet2.Models;

public partial class CroissanceRace
{
    public int IdCroissanceRace { get; set; }

    public int? IdRace { get; set; }

    public double? ApportProteineG { get; set; }

    public double? ApportGlucideG { get; set; }

    public double? PoidsObtenuG { get; set; }

    public virtual Race? IdRaceNavigation { get; set; }
}
