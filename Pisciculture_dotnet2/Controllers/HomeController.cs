using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
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

        ViewBag.DateNourrissage = dateNourrissage;
        ViewBag.Benefice = benefice;

        var model = (ChiffresAffaires: chiffresAffaires, Depenses: depenses);

        return View(model);
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