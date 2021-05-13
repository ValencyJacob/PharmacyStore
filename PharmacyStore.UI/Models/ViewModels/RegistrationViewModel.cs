using System.ComponentModel.DataAnnotations;

namespace PharmacyStore.UI.Models.ViewModels
{
    public class RegistrationViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email address")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(15, ErrorMessage = "Your password is limited to {2} to {1} characters", MinimumLength = 6)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password don't match")]
        public string ConfirmPassword { get; set; }
    }
}
