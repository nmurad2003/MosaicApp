using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MosaicApp.Contexts;
using MosaicApp.Models;
using MosaicApp.ViewModels.PositionVMs;

namespace MosaicApp.Areas.Admin.Controllers;

[Area("Admin")]
public class PositionController(MosaicDbContext _context) : Controller
{
    public async Task<IActionResult> Index()
    {
        List<PositionGetVM> vms = await _context.Positions.Select(p => new PositionGetVM()
        {
            Id = p.Id,
            Name = p.Name,
        }).ToListAsync();

        return View(vms);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost, AutoValidateAntiforgeryToken]
    public async Task<IActionResult> Create(PositionCreateVM model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var position = new Position() { Name = model.Name };

        await _context.Positions.AddAsync(position);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Update(int id)
    {
        var entity = await _context.Positions.FindAsync(id);

        if (entity == null)
            return NotFound();

        var model = new PositionUpdateVM()
        {
            Id = entity.Id,
            Name = entity.Name,
        };
        await _context.SaveChangesAsync();

        return View(model);
    }

    [HttpPost, AutoValidateAntiforgeryToken]
    public async Task<IActionResult> Update(PositionUpdateVM model)
    {
        var entity = await _context.Positions.FindAsync(model.Id);

        if (entity == null)
            return NotFound();
        
        entity.Name = model.Name;
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    [AutoValidateAntiforgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var entity = await _context.Positions.FindAsync(id);

        if (entity == null)
            return NotFound();

        _context.Positions.Remove(entity);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }
}
