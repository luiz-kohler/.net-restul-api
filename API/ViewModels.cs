using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace API
{
    #region User

    public record CreateUserRequest(
        [property: Required] [property: MinLength(2)] string Name,
        [property: Required] [property: EmailAddress] string Email, 
        [property: Required] [property: MinLength(4)] string Password
    );

    public record UpdateUserRequest(
        [property: Required] [property: MinLength(2)] string Name
    );

    public record LoginRequest(
        [property: Required] [property: EmailAddress] string Email,
        [property: Required] [property: MinLength(4)] string Password
    );

    public record BasicUserResponse(int Id, string Name);
    public record DetailedUserResponse(int Id, string Name, IEnumerable<BasicPetResponse> Pets) 
                : BasicUserResponse(Id, Name);

    #endregion

    #region Pet

    public record PetRequest(
        [property: Range(1, int.MaxValue)] int UserId,
        [property: Required] [property: MinLength(2)] string Name,
        [property: Range(1, 999)] int Age, 
        bool IsVaccinated
    );

    public record BasicPetResponse(int Id, int UserId, string Name, int Age, bool IsVaccinated);
    public record DetailedPetResponse(int Id, int UserId, string UserName, string Name, int Age, bool IsVaccinated)
                : BasicPetResponse(Id, UserId, Name, Age, IsVaccinated);

    #endregion
}
