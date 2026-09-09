using VirtualPet.Application.DTOs;

namespace VirtualPet.Web.Models.Pets
{
    public sealed class PetActionAjaxResponse
    {
        public required PetDto Pet { get; init; }
        public bool Evolved { get; init; }
        public string Message { get; init; } = string.Empty;
    }
}
