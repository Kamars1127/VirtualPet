using Microsoft.EntityFrameworkCore;
using VirtualPet.Application.Repositories;
using VirtualPet.Domain.Entities;
using VirtualPet.Infrastructure.Data;

namespace VirtualPet.Infrastructure.Repositories
{
    public sealed class UserRepository : IUserRepository
    {
        private readonly VirtualPetDbContext _dbContext;

        public UserRepository(VirtualPetDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(user => user.Id == id, cancellationToken);
        }

        public async Task AddAsync(User user, CancellationToken cancellation = default)
        {
            ArgumentNullException.ThrowIfNull(user);

            await _dbContext.Users.AddAsync(user, cancellation);
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<User>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.Users
                .AsNoTracking()
                .OrderBy(user => user.CreateAt)
                .ToListAsync(cancellationToken);
        }
    }
}
