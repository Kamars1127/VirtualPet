using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VirtualPet.Domain.Entities;

namespace VirtualPet.Infrastructure.Configurations
{
    public sealed class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");
            builder.HasKey(user => user.Id);
            builder.Property(user => user.Id).ValueGeneratedNever();
            builder.Property(user => user.Name).IsRequired().HasMaxLength(20);
            builder.Property(user => user.CreateAt).IsRequired();
            builder.HasMany(user => user.Pets)
                .WithOne(pet => pet.User)
                .HasForeignKey(pet => pet.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
