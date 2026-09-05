using Microsoft.EntityFrameworkCore;
using VirtualPet.Application.Repositories;
using VirtualPet.Domain.Entities;
using VirtualPet.Infrastructure.Data;

namespace VirtualPet.Infrastructure.Repositories
{
    public sealed class PetHistoryRepository : IPetHistoryRepository
    {
        private readonly VirtualPetDbContext _dbContext;

        public PetHistoryRepository(VirtualPetDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<IReadOnlyList<PetHistory>> GetByPetIdAsync(Guid petId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.PetHistories
                .AsNoTracking()
                .Where(history => history.PetId == petId)
                .OrderByDescending(history => history.CreateAt)
                .ToListAsync(cancellationToken);
        }

        public async Task AddAsync(PetHistory history, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(history);

            await _dbContext.PetHistories.AddAsync(history, cancellationToken);
        }

       
    }
}
