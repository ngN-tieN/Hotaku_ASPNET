namespace Hotaku.Persistence.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<TEntity> AddAsync(TEntity entity);

        Task<TEntity> UpdateAsync(TEntity entity);

        Task<List<TEntity>> GetAll();

        Task<string> DeleteAsync(TEntity entity);

        Task<bool> CheckEntityExistence(TEntity entity);
    }
}
