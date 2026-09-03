using Microsoft.EntityFrameworkCore;
using VirtualPet.Domain.Entities;
using VirtualPet.Infrastructure.Configurations;

namespace VirtualPet.Infrastructure.Data
{
    /// <summary>
    /// VirtualPet 資料庫 DbContext
    /// </summary>
    public class VirtualPetDbContext : DbContext
    {
        public VirtualPetDbContext(DbContextOptions<VirtualPetDbContext> options) : base(options)
        {

        }

        /// <summary>
        /// Pet 資料表
        /// </summary>
        public DbSet<Pet> Pets => Set<Pet>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new PetConfiguration);
        }
    }
}
