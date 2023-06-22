using System.ComponentModel.DataAnnotations;

namespace DemoPL.Models.Account
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email is Required")]
        [EmailAddress(ErrorMessage = "Invalid Email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Email is Required")]
        [MinLength(5, ErrorMessage = "MinLength is 5")]
        public string Password { get; set; }
        public bool RememberMe { get; set; } 
    }
}
