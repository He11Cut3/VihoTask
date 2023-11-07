using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace VihoTask.Models.ViewModels
{
    public class LoginViewModel
    {
            [Required, MinLength(2, ErrorMessage = "Minimum length is 2")]
            [Display(Name = "Username")]
            public string Email { get; set; }

            [DataType(DataType.Password), Required, MinLength(4, ErrorMessage = "Minimum length is 4")]
            public string Password { get; set; }

            public string ReturnUrl { get; set; }
    }
}
