using API.Handlers;
using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using XAct.Messages;

namespace API.Controllers
{
    // Rule: A plural noun should be used for collection names
    [Route("users")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // Rule: The query component of a URI should be used to paginate collection or store results
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int page, [FromQuery] int pageSize, [FromQuery] int cursor) => await _userService.GetAll();

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id) => await _userService.GetById(id);

        // Rule: Hyphens (-) should be used to improve the readability of URIs
        // Rule: Underscores (_) should not be used in URIs
        [HttpGet("first-user")]
        public async Task<IActionResult> GetFirstPetFromUser() => await _userService.GetFirstUser();

        // Rule: File extensions should not be included in URIs
        // Rule: A singular noun should be used for document names
        [HttpGet("report")]
        public async Task<IActionResult> Report() => await _userService.GenerateReport();

        [HttpGet("is-logged")]
        [Authorize]
        public IActionResult IsLogged() => NoContent();

        [HttpGet("is-admin")]
        [Authorize(Roles = Roles.ADMIN)]
        public IActionResult IsAdmin() => NoContent();

        // Rule: CRUD function names should not be used in URIs
        // Rule: POST must be used to create a new resource in a collection
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserRequest request) => await _userService.Create(request);

        // Rule: A verb or verb phrase should be used for controller names
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request) => await _userService.Login(request);

        // Rule: PUT must be used to update mutable resources
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateUserRequest request) => await _userService.Update(id, request);

        // Rule: PUT must be used to both insert and update a stored resource
        [HttpPut("upsert/{id}")]
        public async Task<IActionResult> Upsert([FromRoute] int id, [FromBody] CreateUserRequest request) => await _userService.Upsert(id, request);

        // Rule: DELETE must be used to remove a resource from its parent
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id) => await _userService.Delete(id);

        //Rule: HEAD should be used to retrieve response headers
        [HttpHead("{id}")]
        public async Task<IActionResult> CheckUserExistence([FromRoute] int id) => await _userService.CheckUserExistence(id);

        // Rule: OPTIONS should be used to retrieve metadata that describes a resource’s available interactions
        [HttpOptions]
        public IActionResult Options() => _userService.Options();
    }
}
