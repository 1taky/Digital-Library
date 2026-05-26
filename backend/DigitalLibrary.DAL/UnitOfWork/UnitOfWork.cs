using DigitalLibrary.DAL.Data;
using DigitalLibrary.DAL.Interfaces;

namespace DigitalLibrary.DAL.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly DigitalLibraryDbContext _context;

    public IUserRepository Users { get; }

    public UnitOfWork(
        DigitalLibraryDbContext context,
        IUserRepository users)
    {
        _context = context;
        Users = users;
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
}