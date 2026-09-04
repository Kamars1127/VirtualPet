using VirtualPet.Application.DTOs;
using VirtualPet.Domain.Entities;

namespace VirtualPet.Application.Mappings
{
    public static class PetHistoryMapper
    {
        public static PetHistoryDto ToDto(PetHistory history)
        {
            ArgumentNullException.ThrowIfNull(history);

            return new PetHistoryDto
            {
                Id = history.Id,
                PetId = history.PetId,
                ActionType = history.ActionType,
                Description = history.Description,
                CreateAt = history.CreateAt
            };
        }
    }
}
