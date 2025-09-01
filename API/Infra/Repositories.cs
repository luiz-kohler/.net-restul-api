namespace API.Infra
{
    public interface IUserRepository : IBaseRepository<User> { }

    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(ICollectionAccessor collectionAccessor) : base(collectionAccessor) { }
    }
}
