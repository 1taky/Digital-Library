using DigitalLibrary.DAL.Data;
using DigitalLibrary.DAL.Interfaces;

namespace DigitalLibrary.DAL.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly DigitalLibraryDbContext _context;

    public IUserRepository Users { get; }

    public IGenreRepository Genres { get; }

    public IBookRepository Books { get; }

    public IBookFileRepository BookFiles { get; }

    public IOrderRepository Orders { get; }

    public UnitOfWork(
        DigitalLibraryDbContext context,
        IUserRepository users,
        IGenreRepository genres,
        IBookRepository books,
        IBookFileRepository bookFiles,
        IOrderRepository orders)
    {
        _context = context;
        Users = users;
        Genres = genres;
        Books = books;
        BookFiles = bookFiles;
        Orders = orders;
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
}