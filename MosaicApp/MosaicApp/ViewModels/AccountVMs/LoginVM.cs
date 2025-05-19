using System.ComponentModel.DataAnnotations;

namespace MosaicApp.ViewModels.AccountVMs;

public class LoginVM
{
    [Required(ErrorMessage = "Password is required!")]
    [EmailAddress(ErrorMessage = "Invalid email address!")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Password is required!")]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}
