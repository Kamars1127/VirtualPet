using VirtualPet.Domain.Entities;

namespace VirtualPet.Application.Repositories
{
    public interface IPetHistoryRepository
    {
        Task<IReadOnlyList<PetHistory>> GetByPetIdAsync(Guid petId, CancellationToken cancellationToken = default);

        Task AddAsync(PetHistory history, CancellationToken cancellationToken = default);
    }
}
