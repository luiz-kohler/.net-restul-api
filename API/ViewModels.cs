using System.Xml.Linq;

namespace API
{
    #region User

    public record CreateUserRequest(string Name, string Email, string Password);
    public record UpdateUserRequest(string Name, string Password);
    public record LoginRequest(string Email, string Password);
    public record BasicUserResponse(int Id, string Name);
    public record DetailedUserResponse(int Id, string Name, IEnumerable<BasicPetResponse> Pets) 
                : BasicUserResponse(Id, Name);

    #endregion

    #region Pet

    public record PetRequest(int UserId, string Name, int Age, bool IsVaccinated);
    public record BasicPetResponse(int Id, int UserId, string Name, int Age, bool IsVaccinated);
    public record DetailedPetResponse(int Id, int UserId, string UserName, string Name, int Age, bool IsVaccinated)
                : BasicPetResponse(Id, UserId, Name, Age, IsVaccinated);

    #endregion
}
