using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Pisciculture_dotnet2.Models;
using Pisciculture_dotnet2.Utilities;

namespace Pisciculture_dotnet2.Controllers;

public class HomeController : Controller
{
    private readonly PiscicultureDbContext _context;
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger,PiscicultureDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult Index(DateOnly? dateNourrissage)
    {
        var depenses = NourrissageUtilities.GetNourrissagePrixParDobo(_context,dateNourrissage);
        var benefice = PoissonDoboUtilities.getBenefice(_context, dateNourrissage);
        var chiffresAffaires = PoissonDoboUtilities.getChiffresAffaires(_context, dateNourrissage);

        var ca=chiffresAffaires.Sum(x=>x.ChiffreDaffaires);
        ViewBag.DateNourrissage = dateNourrissage;
        ViewBag.Benefice = benefice;
        ViewBag.CA = ca;
        ViewBag.PrixAchatTotal = ca - benefice;

        var model = (ChiffresAffaires: chiffresAffaires, Depenses: depenses);

        return View(model);
    }
    
    public IActionResult PoissonsDobo(string idDobo, DateTime? dateFiltre)
    {
        DateOnly? dateOnly = dateFiltre.HasValue 
            ? DateOnly.FromDateTime(dateFiltre.Value) 
            : null;
    
        ViewBag.DateFiltre = dateFiltre;
        ViewBag.IdDobo = idDobo;
    
        // Récupérer la liste des dobos pour le dropdown
        ViewBag.Dobos = new SelectList(_context.Dobos, "IdDobo", "IdDobo", idDobo);
    
        List<PoissonPoidsInfo> poissons = new List<PoissonPoidsInfo>();
    
        if (!string.IsNullOrEmpty(idDobo))
        {
            poissons = PoissonDoboUtilities.getAllPoissonsPoidsByIdDobo(_context, idDobo, dateOnly);
        }
    
        // Calculer les totaux
        ViewBag.TotalPoidsInitial = poissons.Sum(p => p.PoidsInitialKg);
        ViewBag.TotalPoidsRecu = poissons.Sum(p => p.PoidsTotalRecuKg);
        ViewBag.TotalPoidsActuel = poissons.Sum(p => p.PoidsActuelleKg);
        ViewBag.TotalChiffreAffaires = poissons.Sum(p => p.ChiffreDaffaires);
        ViewBag.TotalPrixAchat = poissons.Sum(p => p.PrixAchatKg * p.PoidsActuelleKg);
    
        return View(poissons);
    }

    public IActionResult GetCroissanceDetails(string idPoissonDobo, DateTime? dateFiltre)
    {
        DateOnly? dateOnly = dateFiltre.HasValue 
            ? DateOnly.FromDateTime(dateFiltre.Value) 
            : null;
    
        var croissances = CroissancePoissonDoboUtilities.getCroissancesByIdPoissonDobo(_context, idPoissonDobo, dateOnly);
    
        return PartialView("_CroissanceDetails", croissances);
    }
    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}