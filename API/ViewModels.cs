namespace API
{
    #region User

    public record UserRequest(string Name, string Password);
    public record BasicUserResponse(int Id, string Name);
    public record DetailedUserResponse(int Id, int UserId, string UserName, string Name, int Age, bool IsVaccinated, IEnumerable<BasicPetResponse> Pets);

    #endregion

    #region Pet

    public record PetRequest(int UserId, string Name, int Age, bool IsVaccinated);
    public record BasicPetResponse(int Id, int UserId, string Name, int Age, bool IsVaccinated);
    public record DetailedPetResponse(int Id, int UserId, string UserName, string Name, int Age, bool IsVaccinated);

    #endregion

}
