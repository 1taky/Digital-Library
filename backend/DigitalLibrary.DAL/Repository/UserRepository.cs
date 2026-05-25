using DigitalLibrary.DAL.Data;
using DigitalLibrary.DAL.Entities;
using DigitalLibrary.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DigitalLibrary.DAL.Repositories;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    public UserRepository(DigitalLibraryDbContext context)
        : base(context)
    {
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        string normalizedEmail = email.Trim().ToUpper();

        return await DbSet
            .FirstOrDefaultAsync(user => user.NormalizedEmail == normalizedEmail);
    }

    public async Task<bool> ExistsByEmailAsync(string email)
    {
        string normalizedEmail = email.Trim().ToUpper();

        return await DbSet
            .AnyAsync(user => user.NormalizedEmail == normalizedEmail);
    }
}