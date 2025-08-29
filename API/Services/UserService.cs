using Microsoft.AspNetCore.Mvc;

namespace API.Services
{
    public interface IUserService
    {
        Task<IActionResult> Create(CreateUserRequest request);
        Task<IActionResult> GetAll();
        Task<IActionResult> GetById(int id);
        Task<IActionResult> Update(int id, UpdateUserRequest request);
        Task<IActionResult> Delete(int id);
        Task<IActionResult> Login(LoginRequest request);
    }

    public class UserService : IUserService
    {
        public Task<IActionResult> Create(CreateUserRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<IActionResult> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IActionResult> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<IActionResult> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IActionResult> Login(LoginRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<IActionResult> Update(int id, UpdateUserRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
