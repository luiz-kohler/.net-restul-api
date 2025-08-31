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
        public required string Email { get; set; }
    }

    public class DetailedUserResponse : BasicUserResponse
    {
        public required IEnumerable<BasicPetResponse> Pets { get; set; }
    }

    #endregion

    #region Pet

    public class PetRequest
    {
        [Range(1, int.MaxValue)]
        public int UserId { get; set; }

        [Required]
        [MinLength(2)]
        public required string Name { get; set; }

        [Range(1, 999)]
        public int Age { get; set; }

        public bool IsVaccinated { get; set; }
    }

    public class BasicPetResponse
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public required string Name { get; set; }
        public int Age { get; set; }
        public bool IsVaccinated { get; set; }
    }

    public class DetailedPetResponse : BasicPetResponse
    {
        public required string UserName { get; set; }
    }

    #endregion
}
