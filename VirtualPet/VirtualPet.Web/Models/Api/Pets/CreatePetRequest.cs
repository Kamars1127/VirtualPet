using System.ComponentModel.DataAnnotations;
using VirtualPet.Domain.Enums;

namespace VirtualPet.Web.Models.Api.Pets
{
    public sealed class CreatePetRequest
    {
        [Required(ErrorMessage = "Pet name is required.")]
        [StringLength(20, ErrorMessage = "Pet name cannot be longer than 20 characters.")]
        public string Name {  get; set; } = string.Empty;

        [Required(ErrorMessage = "Pet species is required.")]
        public PetSpecies? Species { get; set; }
    }
}
