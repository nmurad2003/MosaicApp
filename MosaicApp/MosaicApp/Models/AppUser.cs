using Microsoft.AspNetCore.Identity;

namespace MosaicApp.Models;

public class AppUser : IdentityUser
{
    public string FullName { get; set; }
}
