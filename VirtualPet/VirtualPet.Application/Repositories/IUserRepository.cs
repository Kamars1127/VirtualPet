using VirtualPet.Domain.Entities;

namespace VirtualPet.Application.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        Task AddAsync(User user, CancellationToken = default);

        Task SaveChangesAsync(CancellationToken = default);
    }
}
