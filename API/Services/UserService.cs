using API.Exceptions;
using API.Handlers;
using API.Infra;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using XAct;
using XAct.Users;

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
        Task<IActionResult> Upsert(int id, CreateUserRequest request);
        Task<IActionResult> GetFirstPetFromUser(int id);
        Task<IActionResult> CheckUserExistence(int id);
        IActionResult Options();
    }

    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPetRepository _petRepository;
        private readonly IHashHandler _hashHandler;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(
            IUserRepository userRepository,
            IPetRepository petRepository,
            IHashHandler hashHandler,
            IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository;
            _petRepository = petRepository;
            _hashHandler = hashHandler;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IActionResult> CheckUserExistence(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);

            _httpContextAccessor.HttpContext?.Response.Headers.Append("User-exists", user is null ? "No" : "Yes");

            return new NoContentResult();
        }

        public async Task<IActionResult> Create(CreateUserRequest request)
        {
            var isEmailAlreadyRegistered = await _userRepository.ExistsAsync(user => user.Email.Equals(request.Email, StringComparison.OrdinalIgnoreCase));

            if (isEmailAlreadyRegistered)
                throw new ConflictException("This email is already registered.");

            var user = new User 
            {
                Email = request.Email,
                Name = request.Name,
                HashedPassword = _hashHandler.Hash(request.Password),
                CreatedAt = DateTime.UtcNow,
                ETag = Guid.NewGuid().ToString().Replace("-", ""),
            };

            await _userRepository.CreateAsync(user);

            return new CreatedResult("users", new {Id = user.Id});
        }

        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);

            if (user is null)
                throw new NotFoundException("User not found.");

            var pets = await _petRepository.GetAllAsync(pet => pet.UserId == user.Id);

            if(pets.Any())
                await _petRepository.DeleteBulkAsync(pets);

            await _userRepository.DeleteAsync(id);

            return new NoContentResult();

        }

        public async Task<IActionResult> GetAll()
        {
            var users = await _userRepository.GetAllAsync();

            var response = users.Map(user => new BasicUserResponse { Id = user.Id, Name = user.Name, Email = user.Email });

            return new OkObjectResult(response);
        }

        public async Task<IActionResult> GetById(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);

            if(user is null)
                throw new NotFoundException("User not found.");

            var pets = await _petRepository.GetAllAsync(pet => pet.UserId == user.Id);

            var response = new DetailedUserResponse 
            { 
                Id = user.Id, 
                Name = user.Name, 
                Email = user.Email, 
                Pets = pets.Map(pet => new BasicPetResponse() 
                { 
                    Id = pet.Id,
                    UserId = user.Id,
                    Age = pet.Age,
                    Name = pet.Name,
                    IsVaccinated = pet.IsVaccinated,
                })  
            };

            return new OkObjectResult(response);
        }

        public async Task<IActionResult> GetFirstPetFromUser(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);

            if (user is null)
                throw new NotFoundException("User not found.");

            var pets = await _petRepository.GetAllAsync(pet => pet.UserId == user.Id);
            var firstPet = pets.OrderByDescending(pet => pet.CreatedAt).FirstOrDefault();

            if(firstPet == null)
                throw new NotFoundException("Pet not found.");

            var response = new BasicPetResponse
            {
                Id = firstPet.Id,
                UserId = user.Id,
                Name = firstPet.Name,
                Age = firstPet.Age,
                IsVaccinated = firstPet.IsVaccinated
            };

            return new OkObjectResult(response);
        }

        public async Task<IActionResult> Login(LoginRequest request)
        {
            var hashedPassword = _hashHandler.Hash(request.Password);

            var user = await _userRepository.GetOneAsync(user => user.Email == request.Email 
                                                              && user.HashedPassword == hashedPassword);

            if(user == null)
                throw new NotFoundException("User not found.");

            return new OkObjectResult(new { Message = "Authenticate with success." });
        }

        public IActionResult Options()
        {
            _httpContextAccessor.HttpContext?.Response.Headers.Append("Allow", "GET, PUT, DELETE, HEAD");
            return new NoContentResult();
        }

        public async Task<IActionResult> Update(int id, UpdateUserRequest request)
        {
            var user = await _userRepository.GetByIdAsync(id);

            if (user is null)
                throw new NotFoundException("User not found.");

            user.Name = request.Name;

            await _userRepository.UpdateAsync(user);

            return new NoContentResult();
        }

        public async Task<IActionResult> Upsert(int id, CreateUserRequest request)
        {
            var user = await _userRepository.GetByIdAsync(id);

            if (user == null)
                return await Create(request);
            else
                return await Update(id, new() { Name = request.Name });
        }
    }
}
