using DigitalLibrary.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DigitalLibrary.DAL.Data;

public class DigitalLibraryDbContext : DbContext
{
    public DigitalLibraryDbContext(DbContextOptions<DigitalLibraryDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("users");

            entity.HasKey(user => user.Id);

            entity.Property(user => user.FullName)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(user => user.Email)
                .IsRequired()
                .HasMaxLength(150);

            entity.Property(user => user.NormalizedEmail)
                .IsRequired()
                .HasMaxLength(150);

            entity.HasIndex(user => user.NormalizedEmail)
                .IsUnique();

            entity.Property(user => user.PasswordHash)
                .IsRequired();

            entity.Property(user => user.Role)
                .IsRequired()
                .HasConversion<string>();

            entity.Property(user => user.IsActive)
                .IsRequired();

            entity.Property(user => user.CreatedAt)
                .IsRequired();
        });
    }
}