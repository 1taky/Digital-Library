namespace DigitalLibrary.DAL.Interfaces;

public interface IUnitOfWork
{
    IUserRepository Users { get; }

    IGenreRepository Genres { get; }

    Task<int> SaveChangesAsync();
}