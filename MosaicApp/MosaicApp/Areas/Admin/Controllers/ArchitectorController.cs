using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using MosaicApp.Contexts;
using MosaicApp.Models;
using MosaicApp.ViewModels.ArchitectorVMs;
using MosaicApp.ViewModels.PositionVMs;

namespace MosaicApp.Areas.Admin.Controllers;

[Area("Admin")]
public class ArchitectorController(MosaicDbContext _context, IWebHostEnvironment _env) : Controller
{
    public async Task<IActionResult> Index()
    {
        List<ArchitectorGetVM> vms = await _context.Architectors.Select(a => new ArchitectorGetVM()
        {
            Id = a.Id,
            FirstName = a.FirstName,
            LastName = a.LastName,
            Description = a.Description,
            ImagePath = a.ImagePath,
            Position = new PositionGetVM() { Id = a.Position.Id, Name = a.Position.Name },
        }).ToListAsync();

        return View(vms);
    }

    public async Task<IActionResult> Create()
    {
        await FillPositionsToViewBagAsync();

        return View();
    }

    [HttpPost, AutoValidateAntiforgeryToken]
    public async Task<IActionResult> Create(ArchitectorCreateVM model)
    {
        if (!ModelState.IsValid)
        {
            await FillPositionsToViewBagAsync();
            return View(model);
        }

        if (await _context.Positions.FindAsync(model.PositionId) == null)
        {
            ModelState.AddModelError("PositionId", "Teacher not found!");
            await FillPositionsToViewBagAsync();
            return View(model);
        }

        string? imagePath = null;

        if (model.Image != null)
        {
            if (model.Image.Length > 2 * 1024 * 1024)
            {
                ModelState.AddModelError("Image", "Image size cannot exceed 2 MBs");
                await FillPositionsToViewBagAsync();
                return View(model);
            }

            if (!model.Image.ContentType.StartsWith("image"))
            {
                ModelState.AddModelError("Image", "Only image files are accepted");
                await FillPositionsToViewBagAsync();
                return View(model);
            }

            imagePath = await CopyToNewImagePathAsync(model.Image);
        }

        var entity = new Architector()
        {
            FirstName = model.FirstName,
            LastName = model.LastName,
            Description = model.Description,
            PositionId = model.PositionId,
            ImagePath = imagePath,
        };

        await _context.Architectors.AddAsync(entity);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Update(int id)
    {
        Architector? entity = await _context.Architectors.FindAsync(id);

        if (entity == null)
            return NotFound();

        await FillPositionsToViewBagAsync();

        var model = new ArchitectorUpdateVM()
        {
            Id = entity.Id,
            FirstName = entity.FirstName,
            LastName = entity.LastName,
            Description = entity.Description,
            ImagePath = entity.ImagePath,
            PositionId = entity.PositionId,
        };

        return View(model);
    }

    [HttpPost, AutoValidateAntiforgeryToken]
    public async Task<IActionResult> Update(ArchitectorUpdateVM model)
    {
        if (!ModelState.IsValid)
        {
            ModelState.AddModelError("PositionId", "Teacher not found!");
            await FillPositionsToViewBagAsync();
            return View(model);
        }

        if (await _context.Positions.FindAsync(model.PositionId) == null)
        {
            ModelState.AddModelError("PositionId", "Teacher not found!");
            await FillPositionsToViewBagAsync();
            return View(model);
        }

        Architector? entity = await _context.Architectors.FindAsync(model.Id);

        if (entity == null)
            return NotFound();

        string? imagePath = entity.ImagePath;

        if (model.Image != null)
        {
            if (model.Image.Length > 2 * 1024 * 1024)
            {
                ModelState.AddModelError("Image", "Image size cannot exceed 2 MBs");
                await FillPositionsToViewBagAsync();
                return View(model);
            }

            if (!model.Image.ContentType.StartsWith("image"))
            {
                ModelState.AddModelError("Image", "Only image files are accepted");
                await FillPositionsToViewBagAsync();
                return View(model);
            }

            if (entity.ImagePath == null)
                imagePath = await CopyToNewImagePathAsync(model.Image);
            else
                await CopyToExistingImagePathAsync(model.Image, entity.ImagePath);
        }

        entity.FirstName = model.FirstName;
        entity.LastName = model.LastName;
        entity.Description = model.Description;
        entity.PositionId = model.PositionId;
        entity.ImagePath = imagePath;

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    [AutoValidateAntiforgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        Architector? entity = await _context.Architectors.FindAsync(id);

        if (entity == null)
            return NotFound();

        if (entity.ImagePath != null)
        {
            string fullPath = _env.WebRootPath + entity.ImagePath;
            if (System.IO.File.Exists(fullPath))
                System.IO.File.Delete(fullPath);
        }

        _context.Architectors.Remove(entity);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }


    // Utility method
    public async Task<string> CopyToNewImagePathAsync(IFormFile image)
    {
        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
        string fullPath = Path.Combine(_env.WebRootPath, "uploads", fileName);

        using var fs = new FileStream(fullPath, FileMode.Create);
        await image.CopyToAsync(fs);

        return "/uploads/" + fileName;
    }

    public async Task CopyToExistingImagePathAsync(IFormFile image, string imagePath)
    {
        string fullPath = _env.WebRootPath + imagePath;

        using var fs = new FileStream(fullPath, FileMode.Create);
        await image.CopyToAsync(fs);
    }

    public async Task FillPositionsToViewBagAsync()
    {
        ViewBag.Positions = await _context.Positions.Select(p => new PositionGetVM()
        {
            Id = p.Id,
            Name = p.Name,
        }).ToListAsync();
    }
}
