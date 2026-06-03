using DigitalLibrary.DAL.Entities;

namespace DigitalLibrary.DAL.Interfaces;

public interface IGenreRepository : IGenericRepository<Genre>
{
    Task<bool> ExistsByNameAsync(string name);
}