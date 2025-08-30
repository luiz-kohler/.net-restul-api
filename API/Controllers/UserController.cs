using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    // Rule: A plural noun should be used for collection names
    [Route("users")]
    public class UserController : ControllerBase
    {
        // Rule: CRUD function names should not be used in URIs
        // Rule: POST must be used to create a new resource in a collection
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserRequest request)
        {
            return Ok();
        }

        // Rule: The query component of a URI should be used to paginate collection or store results
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int page, [FromQuery] int pageSize, [FromQuery] int cursor)
        {
            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            return Ok();
        }

        // Rule: PUT must be used to update mutable resources
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateUserRequest request)
        {
            return Ok();
        }

        // Rule: PUT must be used to both insert and update a stored resource
        [HttpPut("upsert/{id}")]
        public async Task<IActionResult> Upsert([FromRoute] int id, [FromBody] UpdateUserRequest request)
        {
            return Ok();
        }

        // Rule: DELETE must be used to remove a resource from its parent
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            return Ok();
        }

        // Rule: Hyphens (-) should be used to improve the readability of URIs
        // Rule: Underscores (_) should not be used in URIs
        [HttpGet("{id}/first-pet")]
        public async Task<IActionResult> DeletePetsFromUser([FromRoute] int id)
        {
            return Ok();
        }

        // Rule: File extensions should not be included in URIs
        // Rule: A singular noun should be used for document names
        [HttpPost("report")]
        public async Task<IActionResult> Report()
        {
            return Ok();
        }

        // Rule: A verb or verb phrase should be used for controller names
        [HttpPost("order-by-name")]
        public async Task<IActionResult> OrderByName()
        {
            return Ok();
        }

        //Rule: HEAD should be used to retrieve response headers
        [HttpHead("{id}")]
        public async Task<IActionResult> CheckUserExistence([FromRoute] int id)
        {
            return Ok();
        }

        // Rule: OPTIONS should be used to retrieve metadata that describes a resource’s available interactions
        [HttpOptions]
        public async Task<IActionResult> Options()
        {
            return Ok();
        }
    }
}
