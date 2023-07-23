using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Actively.Models
{
    public class RegisterUser
    {
        [Required(ErrorMessage = "Username is required")]
        public string? Username { get; set; } //ok

        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        public string? Email { get; set; }//ok

        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }//ok

        [Compare("Password", ErrorMessage = "Password and confirmation password do not match")]
        [Required(ErrorMessage = "ConfirmPassword is required")]
        public string? ConfirmPassword { get; set; } = null!;//ok

        [Required(ErrorMessage = "FirstName is required")]
        public string? FirstName { get; set; } //ok

        [Required(ErrorMessage = "LastName is required")]
        public string? LastName { get; set; }//ok

        [Required(ErrorMessage = "Gender is required")]
        public Gender Gender { get; set; }//ok

        public string? UserAvatar { get; set; }//ok
        public string? Address { get; set; }
    }

    public enum Gender
    {
        Male,
        Female
    }
}
