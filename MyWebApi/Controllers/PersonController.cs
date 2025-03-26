using Microsoft.AspNetCore.Mvc;
using MyWebApi.Models;
using MyWebApi.Repositories;

namespace MyWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly PersonRepository _personRepository;

        public PersonController(PersonRepository personRepository)
        {
            _personRepository = personRepository;
        }

        // ✅ GET ALL Persons
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var persons = await _personRepository.GetAllAsync();
            return Ok(persons);
        }

        // ✅ GET Person by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var person = await _personRepository.GetByIdAsync(id);
            if (person == null)
            {
                return NotFound(new { message = "Person not found" });
            }
            return Ok(person);
        }

        // ✅ CREATE a new Person (also sends an email)
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Person person)
        {
            if (person == null)
            {
                return BadRequest(new { message = "Invalid data" });
            }

            await _personRepository.CreateAsync(person);
            return CreatedAtAction(nameof(GetById), new { id = person.Id }, person);
        }

        // ✅ UPDATE Person by ID
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] Person person)
        {
            var existingPerson = await _personRepository.GetByIdAsync(id);
            if (existingPerson == null)
            {
                return NotFound(new { message = "Person not found" });
            }

            person.Id = id;
            await _personRepository.UpdateAsync(id, person);
            return Ok(new { message = "Person updated successfully" });
        }

        // ✅ DELETE Person by ID
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var existingPerson = await _personRepository.GetByIdAsync(id);
            if (existingPerson == null)
            {
                return NotFound(new { message = "Person not found" });
            }

            await _personRepository.DeleteAsync(id);
            return Ok(new { message = "Person deleted successfully" });
        }
    }
}
