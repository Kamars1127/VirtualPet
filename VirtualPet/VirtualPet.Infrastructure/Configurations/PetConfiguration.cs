using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VirtualPet.Domain.Entities;

namespace VirtualPet.Infrastructure.Configurations
{
    /// <summary>
    /// Pet Entity 的 EF Core 資料庫映射設定
    /// </summary>
    public class PetConfiguration : IEntityTypeConfiguration<Pet>
    {
        public void Configure(EntityTypeBuilder<Pet> builder)
        {
            builder.ToTable("Pets");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).ValueGeneratedNever();
            builder.Property(p => p.Name).IsRequired().HasMaxLength(20);
            builder.Property(p => p.Species).IsRequired();
            builder.Property(p => p.EvolutionStage).IsRequired();
            builder.Property(p => p.State).IsRequired();
            builder.Property(p => p.Level).IsRequired();
            builder.Property(p => p.Experience).IsRequired();
            builder.Property(p => p.CreateAt).IsRequired();
            builder.OwnsOne(p => p.Status, statusBuilder =>
            {
                statusBuilder.Property(s => s.Satiety).IsRequired();
                statusBuilder.Property(s => s.Happiness).IsRequired();
                statusBuilder.Property(s => s.Energy).IsRequired();
            });

            builder.HasMany(pet => pet.Histories)
                .WithOne(history => history.pet)
                .HasForeignKey(history => history.PetId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
