using DigitalLibrary.DAL.Data;
using DigitalLibrary.DAL.Interfaces;

namespace DigitalLibrary.DAL.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly DigitalLibraryDbContext _context;

    public IUserRepository Users { get; }

    public IGenreRepository Genres { get; }

    public IBookRepository Books { get; }

    public UnitOfWork(
        DigitalLibraryDbContext context,
        IUserRepository users,
        IGenreRepository genres,
        IBookRepository books)
    {
        _context = context;
        Users = users;
        Genres = genres;
        Books = books;
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
}