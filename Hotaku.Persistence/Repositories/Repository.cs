using Hotaku.Persistence.Entities;

namespace Hotaku.Persistence.Repositories
{
    public class Repository<TEntity>(HotakuContext hotakuContext) : IRepository<TEntity> where TEntity : class, new()
    {
        protected readonly HotakuContext HotakuContext = hotakuContext;

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            ArgumentNullException.ThrowIfNull(entity);

            try
            {
                await HotakuContext.AddAsync(entity);
                await HotakuContext.SaveChangesAsync();

                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception($"{nameof(entity)} could not be saved: {ex.Message}");
            }
        }

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            ArgumentNullException.ThrowIfNull(entity);

            try
            {
                HotakuContext.Update(entity);
                await HotakuContext.SaveChangesAsync();

                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception($"{nameof(entity)} could not be updated: {ex.Message}");
            }
        }

        public IQueryable<TEntity> GetAll()
        {
            try
            {
                return HotakuContext.Set<TEntity>();
            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't retrieve entities: {ex.Message}");
            }
        }
    }
}
