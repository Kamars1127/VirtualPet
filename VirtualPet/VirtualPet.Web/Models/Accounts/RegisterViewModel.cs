using System.ComponentModel.DataAnnotations;

namespace VirtualPet.Web.Models.Accounts
{
    public sealed class RegisterViewModel
    {
        [Display(Name = "Player Name")]
        [Required(ErrorMessage = "Player name is required.")]
        [StringLength(20, ErrorMessage = "Player name cannot be longer than 20 characters.")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Email")]
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; } = string.Empty;

        [Display(Name = "Password")]
        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "Confirm Password")]
        [Required(ErrorMessage = "Please confirm your password.")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
