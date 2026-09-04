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
            return await _dbContext
        }

        public Task AddAsync(User user, CancellationToken  = default)
        {
            throw new NotImplementedException();
        }

        

        public Task SaveChangesAsync(CancellationToken  = default)
        {
            throw new NotImplementedException();
        }
    }
}
