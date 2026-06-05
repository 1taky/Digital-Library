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
    public DbSet<BookFile> BookFiles => Set<BookFile>();
    public DbSet<BookFormat> BookFormats => Set<BookFormat>();
    public DbSet<Order> Orders => Set<Order>();

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
        });

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

            entity.Property(book => book.Language)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(book => book.PublicationYear)
                .IsRequired();

            entity.Property(book => book.CreatedAt)
                .IsRequired();

            entity.HasOne(book => book.Genre)
                .WithMany(genre => genre.Books)
                .HasForeignKey(book => book.GenreId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(book => new
            {
                book.Title,
                book.Author,
                book.Language,
                book.PublicationYear
            })
            .IsUnique();
        });

        modelBuilder.Entity<BookFile>(entity =>
        {
            entity.ToTable("book_files");

            entity.HasKey(file => file.Id);

            entity.Property(file => file.FileName)
                .IsRequired()
                .HasMaxLength(255);

            entity.Property(file => file.FilePath)
                .IsRequired()
                .HasMaxLength(500);

            entity.Property(file => file.ContentType)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(file => file.FileSize)
                .IsRequired();

            entity.Property(file => file.FileCategory)
                .IsRequired()
                .HasConversion<string>();

            entity.Property(file => file.UploadedAt)
                .IsRequired();

            entity.HasOne(file => file.Book)
                .WithMany(book => book.Files)
                .HasForeignKey(file => file.BookId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<BookFormat>(entity =>
        {
            entity.ToTable("book_formats");

            entity.HasKey(format => format.Id);

            entity.Property(format => format.FormatType)
                .IsRequired()
                .HasConversion<string>();

            entity.Property(format => format.IsAvailable)
                .IsRequired();

            entity.HasOne(format => format.Book)
                .WithMany(book => book.Formats)
                .HasForeignKey(format => format.BookId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(format => new
            {
                format.BookId,
                format.FormatType
            })
            .IsUnique();
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.ToTable("orders");

            entity.HasKey(order => order.Id);

            entity.Property(order => order.PhoneNumber)
                .IsRequired()
                .HasMaxLength(30);

            entity.Property(order => order.Status)
                .IsRequired()
                .HasConversion<string>();

            entity.Property(order => order.CreatedAt)
                .IsRequired();

            entity.HasOne(order => order.User)
                .WithMany()
                .HasForeignKey(order => order.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(order => order.Book)
                .WithMany()
                .HasForeignKey(order => order.BookId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(order => order.Manager)
                .WithMany()
                .HasForeignKey(order => order.ManagerId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}