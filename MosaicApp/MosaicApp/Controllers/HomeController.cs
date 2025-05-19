using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MosaicApp.Contexts;
using MosaicApp.Models;
using MosaicApp.ViewModels.ArchitectorVMs;
using MosaicApp.ViewModels.PositionVMs;

namespace MosaicApp.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly MosaicDbContext _context;

    public HomeController(ILogger<HomeController> logger, MosaicDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        List<ArchitectorGetVM> vms = await _context.Architectors.Select(a => new ArchitectorGetVM()
        {
            Id = a.Id,
            FirstName = a.FirstName,
            LastName = a.LastName,
            Description = a.Description,
            ImagePath = a.ImagePath,
            Position = new PositionGetVM { Id = a.PositionId, Name = a.Position.Name },
        }).ToListAsync();

        return View(vms);
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
