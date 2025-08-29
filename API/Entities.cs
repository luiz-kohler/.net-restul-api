namespace API
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }
        public DateTimeOffset? LastModifiedAt { get; set; }
        public required string ETag { get; set; }
    }

    public class User : BaseEntity
    {
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string HashedPassword { get; set; }
    }

    public class Pet : BaseEntity
    {
        public int UserId { get; set; }
        public required string Name { get; set; }
        public int Age { get; set; }
        public bool IsVaccinated { get; set; }
    }
}