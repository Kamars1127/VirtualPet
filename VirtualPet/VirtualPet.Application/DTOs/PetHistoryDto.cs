using VirtualPet.Domain.Enums;

namespace VirtualPet.Application.DTOs
{
    /// <summary>
    /// Pet 歷史紀錄 DTO
    /// </summary>
    public sealed class PetHistoryDto
    {
        public Guid Id { get; init; }

        public Guid PetId { get; init; }

        public PetHistoryType ActionType { get; init; }

        public string Description { get; init; } = string.Empty;

        public DateTime CreateAt { get; init;  }
    }
}
