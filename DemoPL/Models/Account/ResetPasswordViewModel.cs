using System.ComponentModel.DataAnnotations;

namespace DemoPL.Models.Account
{
    public class ResetPasswordViewModel
    {
        public string Password { get; set; }
        [Required(ErrorMessage = "Confirm Password is Required")]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        public string Email { get; set; }

        public string Token { get; set; }

    }
}
