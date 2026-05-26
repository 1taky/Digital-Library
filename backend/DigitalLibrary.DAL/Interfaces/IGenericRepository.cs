namespace DigitalLibrary.DAL.Interfaces;

public interface IGenericRepository<TEntity> where TEntity : class
{
    Task<TEntity?> GetByIdAsync(int id);

    Task<List<TEntity>> GetAllAsync();

    Task AddAsync(TEntity entity);

    void Update(TEntity entity);

    void Delete(TEntity entity);
}