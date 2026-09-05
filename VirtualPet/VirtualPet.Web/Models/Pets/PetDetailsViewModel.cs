using VirtualPet.Application.DTOs;

namespace VirtualPet.Web.Models.Pets
{
    public sealed class PetDetailsViewModel
    {
        public required PetDto Pet { get; init; }

        public IReadOnlyList<PetHistoryDto> Histories { get; init; } = [];
    }
}
