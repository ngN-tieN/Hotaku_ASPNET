namespace Hotaku.Persistence.Repositories
{
    public interface IRepository<TEntity> where TEntity : class, new()
    {
        Task<TEntity> AddAsync(TEntity entity);

        Task<TEntity> UpdateAsync(TEntity entity);

        IQueryable<TEntity> GetAll();
    }
}
