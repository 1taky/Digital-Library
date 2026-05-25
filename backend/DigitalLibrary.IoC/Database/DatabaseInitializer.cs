using DigitalLibrary.DAL.Data;
using DigitalLibrary.DAL.Entities;
using DigitalLibrary.DAL.Enums;
using Microsoft.EntityFrameworkCore;

namespace DigitalLibrary.IoC.Database;

public class DatabaseInitializer
{
    private readonly DigitalLibraryDbContext _context;

    public DatabaseInitializer(DigitalLibraryDbContext context)
    {
        _context = context;
    }

    public async Task InitializeAsync()
    {
        await _context.Database.EnsureCreatedAsync();

        bool adminExists = await _context.Users
            .AnyAsync(user => user.Role == Role.Admin);

        if (!adminExists)
        {
            User admin = new User
            {
                FullName = "System Admin",
                Email = "admin@digitallibrary.com",
                NormalizedEmail = "ADMIN@DIGITALLIBRARY.COM",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin12345"),
                Role = Role.Admin,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            await _context.Users.AddAsync(admin);
            await _context.SaveChangesAsync();
        }
    }
}