using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("pets")]
    public class PetController : ControllerBase
    {
        [HttpPost]
        public IActionResult Create([FromBody] PetRequest request)
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
        public IActionResult Get([FromRoute] int id, [FromBody] PetRequest request)
        {
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            return Ok();
        }
    }
}
