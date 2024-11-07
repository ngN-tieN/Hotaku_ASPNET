using Hotaku.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Hotaku.Shared.CustomAttribute;
namespace Hotaku.Persistence.Repositories
{
    [Scoped]
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly HotakuContext HotakuContext;

        public Repository(HotakuContext hotakuContext)
        {
            HotakuContext = hotakuContext;
        }

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
                if(!await CheckEntityExistence(entity))
                {
                    throw new Exception("Entity does not exist.");
                }

                var entry = HotakuContext.Entry(entity);
                if (entry.State == EntityState.Detached)
                {
                    HotakuContext.Set<TEntity>().Attach(entity);
                }

                foreach (var property in entry.OriginalValues.Properties)
                {
                    var originalValue = entry.OriginalValues[property];
                    var currentValue = entry.CurrentValues[property];

                    if (!Equals(originalValue, currentValue))
                    {
                        entry.Property(property.Name).IsModified = true;
                    }
                }

                await HotakuContext.SaveChangesAsync();

                await entry.ReloadAsync();

                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception($"Entity could not be updated: {ex.Message}");
            }
        }

        public Task<List<TEntity>> GetAll()
        {
            try
            {
                var entities = HotakuContext.Set<TEntity>().ToList();
                return Task.FromResult(entities);
            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't retrieve entities: {ex.Message}");
            }
        }

        public async Task<string> DeleteAsync(TEntity entity)
        {
            ArgumentNullException.ThrowIfNull(entity);

            try
            {
                if (!await CheckEntityExistence(entity))
                {
                    throw new Exception("Entity does not exist.");
                }

                var entry = HotakuContext.Entry(entity);
                if (entry.State == EntityState.Detached)
                {
                    HotakuContext.Set<TEntity>().Attach(entity);
                }

                HotakuContext.Set<TEntity>().Remove(entity);
                await HotakuContext.SaveChangesAsync();

                return "Entity has been successfully deleted.";
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting entity: {ex.Message}");
            }
        }

        public async Task<bool> CheckEntityExistence(TEntity entity)
        {
            // Get primary key of entity then check if exist
            var primaryKey = HotakuContext.Model
                ?.FindEntityType(typeof(TEntity))
                ?.FindPrimaryKey()
                ?.Properties
                .Select(p => entity.GetType().GetProperty(p.Name)?.GetValue(entity))
                .ToArray();
            var exists = await HotakuContext.Set<TEntity>().FindAsync(primaryKey);

            if (exists == null)
            {
                return false;
            }

            // Make Entity not being tracked by context
            HotakuContext.Entry(exists).State = EntityState.Detached;
            return true;
        }
    }
}

