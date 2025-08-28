using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("users")]
    public class UserController : ControllerBase
    {
        [HttpPost]
        public IActionResult Create([FromBody] CreateUserRequest request)
        {
            return Ok();
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok();
        }

        [HttpGet("{id}")]
        public IActionResult Get([FromRoute] int id)
        {
            return Ok();
        }

        [HttpPut]
        public IActionResult Get([FromRoute] int id, [FromBody] UpdateUserRequest request)
        {
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            return Ok();
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] CreateUserRequest request)
        {
            return Ok();
        }
    }
}
