using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace VihoTask.Models
{
    public class UserModel
    {
            public long Id { get; set; }

            [Required, MinLength(2, ErrorMessage = "Minimum length is 2")]
            [Display(Name = "Username")]
            public string UserName { get; set; }
            
            public byte[] Banner { get; set; }

            public string PostUser { get; set; }
            
            public string UserAbout { get; set; }

            [Required, EmailAddress]
            public string Email { get; set; }
            public byte[] Photo { get; set; }

            [DataType(DataType.Password), Required, MinLength(4, ErrorMessage = "Minimum length is 4")]


            public string Password { get; set; }
    }
}
