namespace API
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }
        public DateTimeOffset? LastModifiedAt { get; set; }
        public required string ETag { get; set; }
        public required DateTimeOffset CreatedAt { get; set; }
    }

    public class User : BaseEntity
    {
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string HashedPassword { get; set; }
    }
}