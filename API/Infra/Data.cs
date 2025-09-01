namespace API.Infra
{
    public interface IData
    {
        public IList<User> Users { get; set; }
    }

    public class Data : IData
    {
        public IList<User> Users { get; set; } = new List<User>();
    }

    public interface ICollectionAccessor
    {
        IList<TEntity> GetCollection<TEntity>() where TEntity : BaseEntity;
    }

    public class CollectionAccessor : ICollectionAccessor
    {
        private readonly IData _data;

        public CollectionAccessor(IData data)
        {
            _data = data;
        }

        public IList<TEntity> GetCollection<TEntity>() where TEntity : BaseEntity
        {
            return typeof(TEntity).Name switch
            {
                nameof(User) => (IList<TEntity>)_data.Users,
                _ => throw new InvalidOperationException($"No collection defined for entity type {typeof(TEntity).Name}")
            };
        }
    }
}

