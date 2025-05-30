﻿using System.ComponentModel.DataAnnotations;

namespace MosaicApp.ViewModels.AccountVMs;

public class RegisterVM
{
    [Required(ErrorMessage = "Username is required!")]
    public string Username { get; set; }

    [Required(ErrorMessage = "Fullname is required!")]
    public string FullName { get; set; }

    [Required(ErrorMessage = "Password is required!")]
    [EmailAddress(ErrorMessage = "Invalid email address!")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Password is required!")]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Required(ErrorMessage = "Confirm Password is required!")]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Passwords do not match!")]
    public string ConfirmPassword { get; set; }
}
