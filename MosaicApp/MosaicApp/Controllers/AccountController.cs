using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MosaicApp.Models;
using MosaicApp.ViewModels.AccountVMs;

namespace MosaicApp.Controllers;

public class AccountController(RoleManager<IdentityRole> _roleManager, UserManager<AppUser> _userManager, SignInManager<AppUser> _signInManager) : Controller
{
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterVM model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var user = new AppUser()
        {
            UserName = model.Username,
            Email = model.Email,
            FullName = model.FullName,
        };
        
        var result = await _userManager.CreateAsync(user, model.Password);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);
            return View(model);
        }

        await _userManager.AddToRoleAsync(user, "Member");
        await _signInManager.SignInAsync(user, isPersistent: false);

        return RedirectToAction(nameof(Login));
    }

    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginVM model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null)
        {
            ModelState.AddModelError("", "Invalid email or password!");
            return View(model);
        }

        var result = await _signInManager.PasswordSignInAsync(user, model.Password, isPersistent: false, lockoutOnFailure: true);
        if (!result.Succeeded)
        {
            ModelState.AddModelError("", "Invalid email or password!");
            return View(model);
        }

        return RedirectToAction("Index", "Home");
    }

    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }


    // Initializing Roles and Admins
    public async Task<IActionResult> CreateRoles()
    {
        await _roleManager.CreateAsync( new IdentityRole() { Name = "admin" });
        await _roleManager.CreateAsync( new IdentityRole() { Name = "member" });
        await _roleManager.CreateAsync( new IdentityRole() { Name = "admin" });

        return Ok("Roles Created!");
    }

    public async Task<IActionResult> CreateAdmin()
    {
        var admin = new AppUser()
        {
            UserName = "admin",
            FullName = "admin",
            Email = "admin@gmail.com",
        };

        await _userManager.CreateAsync(admin, "admin1234");
        await _userManager.AddToRoleAsync(admin, "admin");

        return Ok("Created Admins!");
    }
}
