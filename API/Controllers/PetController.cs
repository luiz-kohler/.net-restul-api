using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("pets")]
    public class PetController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PetRequest request)
        {
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Get([FromRoute] int id, [FromBody] PetRequest request)
        {
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            return Ok();
        }
    }
}
