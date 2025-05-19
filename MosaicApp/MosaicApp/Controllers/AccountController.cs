using Microsoft.AspNetCore.Mvc;

namespace MosaicApp.Controllers;

public class AccountController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
