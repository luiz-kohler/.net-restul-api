using API.Exceptions;
using API.Handlers;
using API.Infra;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using XAct;

namespace API.Services
{
    public interface IUserService
    {
        Task<IActionResult> Create(CreateUserRequest request);
        Task<IActionResult> GetAll();
        Task<IActionResult> GetById(int id);
        Task<IActionResult> GetFirstUser();
        Task<IActionResult> Update(int id, UpdateUserRequest request);
        Task<IActionResult> Delete(int id);
        Task<IActionResult> Login(LoginRequest request);
        Task<IActionResult> Upsert(int id, CreateUserRequest request);
        Task<IActionResult> CheckUserExistence(int id);
        IActionResult Options();
        Task<IActionResult> GenerateReport();
    }

    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IHashHandler _hashHandler;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(
            IUserRepository userRepository,
            IHashHandler hashHandler,
            IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository;
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

            return new CreatedResult("users", new { Id = user.Id });
        }

        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);

            if (user is null)
                throw new NotFoundException("User not found.");

            await _userRepository.DeleteAsync(id);

            return new NoContentResult();

        }

        public async Task<IActionResult> GenerateReport()
        {
            var users = await _userRepository.GetAllAsync();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Users");

                worksheet.Cell(1, 1).Value = "ID";
                worksheet.Cell(1, 2).Value = "Name";
                worksheet.Cell(1, 3).Value = "Email";
                worksheet.Cell(1, 4).Value = "Created At";

                var headerRange = worksheet.Range(1, 1, 1, 4);
                headerRange.Style.Font.Bold = true;
                headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;

                int row = 2;
                foreach (var user in users)
                {
                    worksheet.Cell(row, 1).Value = user.Id;
                    worksheet.Cell(row, 2).Value = user.Name;
                    worksheet.Cell(row, 3).Value = user.Email;
                    worksheet.Cell(row, 4).Value = user.CreatedAt.ToString("d");
                    row++;
                }

                worksheet.Columns().AdjustToContents();

                using (var memoryStream = new MemoryStream())
                {
                    workbook.SaveAs(memoryStream);
                    memoryStream.Position = 0;

                    return new FileContentResult(memoryStream.ToArray(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                    {
                        FileDownloadName = $"Users_Report_{DateTime.UtcNow:yyyyMMdd_HHmmss}.xlsx"
                    };
                }
            }
        }

        public async Task<IActionResult> GetAll()
        {
            var users = await _userRepository.GetAllAsync();

            var response = users.Map(user => new BasicUserResponse { Id = user.Id, Name = user.Name });

            return new OkObjectResult(response);
        }

        public async Task<IActionResult> GetById(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);

            if (user is null)
                throw new NotFoundException("User not found.");

            var response = new DetailedUserResponse
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
            };

            return new OkObjectResult(response);
        }

        public async Task<IActionResult> GetFirstUser()
        {
            var users = await _userRepository.GetAllAsync();

            if (users == null || !users.Any())
                throw new NotFoundException("User not found.");

            var firstUser = users.OrderBy(user => user.CreatedAt).First();

            var response = new DetailedUserResponse
            {
                Id = firstUser.Id,
                Email = firstUser.Email,
                Name = firstUser.Name
            };

            return new OkObjectResult(response);
        }

        public async Task<IActionResult> Login(LoginRequest request)
        {
            var hashedPassword = _hashHandler.Hash(request.Password);

            var user = await _userRepository.GetOneAsync(user => user.Email == request.Email
                                                              && user.HashedPassword == hashedPassword);

            if (user == null)
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
