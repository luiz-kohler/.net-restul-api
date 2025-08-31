namespace API.Infra
{
    public interface IBaseRepository<TEntity> where TEntity : BaseEntity
    {
        Task CreateAsync(TEntity entity);
        Task<TEntity?> GetByIdAsync(int id);
        Task<IList<TEntity>> GetAllAsync();
        Task<IList<TEntity>> GetAllAsync(Func<TEntity, bool> predicate);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(Func<TEntity, bool> predicate);
    }

    public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly IList<TEntity> Collection;

        public BaseRepository(ICollectionAccessor collectionAccessor)
        {
            Collection = collectionAccessor
                .GetCollection<TEntity>();
        }

        public Task CreateAsync(TEntity entity)
        {
            Collection.Add(entity);

            return Task.CompletedTask;
        }

        public Task DeleteAsync(int id)
        {
            var entity = Collection.First(entity => entity.Id == id);

            if(entity != null) 
                Collection.Remove(entity);

            return Task.CompletedTask;
        }

        public Task<bool> ExistsAsync(Func<TEntity, bool> predicate)
        {
            return Task.FromResult(Collection.Any(predicate));
        }

        public Task<IList<TEntity>> GetAllAsync()
        {
            var entities = Collection;
            return Task.FromResult(Collection);
        }

        public Task<IList<TEntity>> GetAllAsync(Func<TEntity, bool> predicate)
        {
           var entities = Collection.Where(predicate);
            return Task.FromResult<IList<TEntity>>(entities.ToList());
        }

        public Task<TEntity?> GetByIdAsync(int id)
        {
            var entity = Collection.FirstOrDefault(entity => entity.Id == id);
            return Task.FromResult(entity);
        }

        public Task UpdateAsync(TEntity entity)
        {
            var index = FindIndexById(entity.Id);

            if (index != -1)
                Collection[index] = entity;

            return Task.CompletedTask;
        }

        private int FindIndexById(int id)
        {
            for (int i = 0; i < Collection.Count; i++)
                if (Collection[i].Id == id)
                    return i;

            return -1;
        }
    }
}
