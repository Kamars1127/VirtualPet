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

       
        public DbSet<User> Users => Set<User>();

        /// <summary>
        /// Pet 資料表
        /// </summary>
        public DbSet<Pet> Pets => Set<Pet>();

        public DbSet<PetHistory> PetHistories => Set<PetHistory>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new )
            modelBuilder.ApplyConfiguration(new PetConfiguration());
        }
    }
}
