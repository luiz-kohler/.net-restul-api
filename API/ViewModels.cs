using System.ComponentModel.DataAnnotations;

namespace API
{
    #region User

    public class CreateUserRequest
    {
        [Required]
        [MinLength(2)]
        public required string Name { get; set; }

        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        [MinLength(4)]
        public required string Password { get; set; }
    }

    public class UpdateUserRequest
    {
        [Required]
        [MinLength(2)]
        public required string Name { get; set; }
    }

    public class LoginRequest
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        [MinLength(4)]
        public required string Password { get; set; }
    }

    public class BasicUserResponse
    {
        public int Id { get; set; }
        public required string Name { get; set; }
    }

    public class DetailedUserResponse : BasicUserResponse
    {
        public required string Email { get; set; }
    }

    #endregion
}
