namespace API
{
    public class User
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string HashedPassword { get; set; }
        public required string ETag { get; set; }
        public DateTimeOffset? LastModifiedAt { get; set; }
    }

    public class Pet
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public required string Name { get; set; }
        public int Age { get; set; }
        public bool IsVaccinated { get; set; }
        public required string ETag { get; set; }
        public DateTimeOffset? LastModifiedAt { get; set; }
    }
}