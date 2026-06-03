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
    public DbSet<Genre> Genres => Set<Genre>();
    public DbSet<Book> Books => Set<Book>();

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

        modelBuilder.Entity<Genre>(entity =>
{
    entity.ToTable("genres");

    entity.HasKey(genre => genre.Id);

    entity.Property(genre => genre.Name)
        .IsRequired()
        .HasMaxLength(100);

    entity.HasIndex(genre => genre.Name)
        .IsUnique();


    modelBuilder.Entity<Book>(entity =>
{
entity.ToTable("books");

entity.HasKey(book => book.Id);

entity.Property(book => book.Title)
    .IsRequired()
    .HasMaxLength(200);

entity.Property(book => book.Author)
    .IsRequired()
    .HasMaxLength(150);

entity.Property(book => book.Description)
    .IsRequired()
    .HasMaxLength(2000);

entity.Property(book => book.BookType)
    .IsRequired()
    .HasConversion<string>();

entity.Property(book => book.Language)
    .IsRequired()
    .HasMaxLength(50);

entity.Property(book => book.PublicationYear)
    .IsRequired();

entity.Property(book => book.IsAvailable)
    .IsRequired();

entity.Property(book => book.CreatedAt)
    .IsRequired();

entity.HasOne(book => book.Genre)
    .WithMany(genre => genre.Books)
    .HasForeignKey(book => book.GenreId)
    .OnDelete(DeleteBehavior.Restrict);
});
});
    }
}