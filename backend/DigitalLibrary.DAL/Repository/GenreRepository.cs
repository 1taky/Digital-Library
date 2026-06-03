using DigitalLibrary.DAL.Data;
using DigitalLibrary.DAL.Entities;
using DigitalLibrary.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DigitalLibrary.DAL.Repositories;

public class GenreRepository : GenericRepository<Genre>, IGenreRepository
{
    public GenreRepository(DigitalLibraryDbContext context)
        : base(context)
    {
    }

    public async Task<bool> ExistsByNameAsync(string name)
    {
        string normalizedName = name.Trim().ToUpper();

        return await DbSet.AnyAsync(genre =>
            genre.Name.ToUpper() == normalizedName);
    }

    public async Task<Genre?> GetByNameAsync(string name)
{
    string normalizedName = name.Trim().ToUpper();

    return await DbSet.FirstOrDefaultAsync(genre =>
        genre.Name.ToUpper() == normalizedName);
}
}