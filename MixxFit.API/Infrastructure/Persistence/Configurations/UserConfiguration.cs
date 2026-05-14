using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MixxFit.API.Domain.Entities.FitnessProfiles;
using MixxFit.API.Domain.Entities.Users;

namespace MixxFit.API.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(u => u.FirstName)
            .HasMaxLength(50);

        builder.Property(u => u.LastName)
            .HasMaxLength(50);

        builder.Property(u => u.RefreshToken)
            .HasMaxLength(500);

        builder.Property(u => u.UserName)
            .HasMaxLength(20);

        builder.Property(u => u.NormalizedUserName)
            .HasMaxLength(20);
        
        builder.Property(u => u.Email)
            .HasMaxLength(256);

        builder.Property(u => u.NormalizedEmail)
            .HasMaxLength(256);

        builder.Property(u => u.PhoneNumber)
            .HasMaxLength(20);

        builder.HasIndex(p => p.UserName)
            .IsUnique();

        builder.HasIndex(p => p.Email)
            .IsUnique();
        
        builder.Property(p => p.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder
            .HasOne(u => u.FitnessProfile)
            .WithOne(f => f.User)
            .HasForeignKey<FitnessProfile>(f => f.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
    }
}