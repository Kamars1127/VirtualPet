using VirtualPet.Application.DTOs;

namespace VirtualPet.Web.Models.Api.Pets
{
    public sealed class PetActionResponse
    {
        public required PetDto Pet { get; init; }
        public bool Evolved { get; init; }
        public string Message {  get; init; } = string.Empty;
    }
}
