using Microsoft.EntityFrameworkCore;
using VirtualPet.Application.Repositories;
using VirtualPet.Domain.Entities;
using VirtualPet.Infrastructure.Data;

namespace VirtualPet.Infrastructure.Repositories
{
    /// <summary>
    /// 使用 EF core 實作 Pet 資料存取
    /// </summary>
    public sealed class PetRepository : IPetRepository
    {
        private readonly VirtualPetDbContext _dbContext;

        public PetRepository(VirtualPetDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<Pet?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Pets.FirstOrDefaultAsync(pet => pet.Id == id, cancellationToken);
        }

        public async Task<IReadOnlyList<Pet>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.Pets.AsNoTracking().OrderBy(pet => pet.CreateAt).ToListAsync(cancellationToken);
        }

        public async Task AddAsync(Pet pet, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(pet);

            await _dbContext.Pets.AddAsync(pet, cancellationToken);
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
