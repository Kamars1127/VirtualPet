using System.ComponentModel.DataAnnotations;
using VirtualPet.Application.DTOs;
using VirtualPet.Domain.Enums;

namespace VirtualPet.Web.Models.Pets
{
    public sealed class CreatePetViewModel
    {
        [Display(Name = "Player")]
        [Required(ErrorMessage = "Please select a player.")]
        public Guid? UserId { get; set; }

        [Display(Name = "Pet Name")]
        [Required(ErrorMessage = "Pet name is required.")]
        [StringLength(20, ErrorMessage = "Pet name cannot be longer than 20 characters.")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Species")]
        [Required(ErrorMessage = "Please select a species.")]
        public PetSpecies? Species { get; set; }

        public IReadOnlyList<UserDto> Users { get; set; } = [];
    }
}
