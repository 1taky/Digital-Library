using DigitalLibrary.DAL.Entities;

namespace DigitalLibrary.DAL.Interfaces;

public interface IBookRepository : IGenericRepository<Book>
{
    Task<Book?> GetByIdWithGenreAsync(int id);

    Task<List<Book>> GetAllWithGenreAsync();
}