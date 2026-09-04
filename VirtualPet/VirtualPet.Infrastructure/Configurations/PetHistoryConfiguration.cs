using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VirtualPet.Domain.Entities;

namespace VirtualPet.Infrastructure.Configurations
{
    public sealed class PetHistoryConfiguration : IEntityTypeConfiguration<PetHistory>
    {
        public void Configure(EntityTypeBuilder<PetHistory> builder)
        {
            builder.ToTable("PetHistories");
            builder.HasKey(history => history.Id);
            builder.Property(history => history.Id).ValueGeneratedNever();
            builder.Property(history => history.PetId).IsRequired();
            builder.Property(history => history.ActionType).IsRequired();
            builder.Property(history => history.Description).IsRequired().HasMaxLength(200);
            builder.Property(history => history.CreateAt).IsRequired();
        }
    }
}
