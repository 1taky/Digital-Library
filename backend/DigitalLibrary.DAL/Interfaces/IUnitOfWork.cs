namespace DigitalLibrary.DAL.Interfaces;

public interface IUnitOfWork
{
    IUserRepository Users { get; }

    IGenreRepository Genres { get; }

    IBookRepository Books { get; }

    Task<int> SaveChangesAsync();
}