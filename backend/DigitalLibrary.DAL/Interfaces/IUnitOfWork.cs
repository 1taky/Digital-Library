namespace DigitalLibrary.DAL.Interfaces;

public interface IUnitOfWork
{
    IUserRepository Users { get; }

    IGenreRepository Genres { get; }

    IBookRepository Books { get; }

    IBookFileRepository BookFiles { get; }

    Task<int> SaveChangesAsync();
}