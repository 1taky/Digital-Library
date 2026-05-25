namespace DigitalLibrary.DAL.Interfaces;

public interface IUnitOfWork
{
    IUserRepository Users { get; }

    Task<int> SaveChangesAsync();
}