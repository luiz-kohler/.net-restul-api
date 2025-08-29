namespace API.Infra
{
    public interface IUserRepository : IBaseRepository<User> { }
    public interface IPetRepository : IBaseRepository<Pet> { }

    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(ICollectionAccessor collectionAccessor) : base(collectionAccessor) { }
    }

    public class PetRepository : BaseRepository<Pet>, IPetRepository
    {
        public PetRepository(ICollectionAccessor collectionAccessor) : base(collectionAccessor) { }
    }
}
