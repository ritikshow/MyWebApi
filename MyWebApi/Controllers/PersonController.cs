using Microsoft.AspNetCore.Mvc;
using MyWebApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class PersonController : ControllerBase
{
    private readonly PersonService _personService;

    public PersonController(PersonService personService)
    {
        _personService = personService;
    }

    [HttpGet]
    public async Task<ActionResult<List<Person>>> GetAll() => await _personService.GetAllAsync();

    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<Person>> GetById(string id)
    {
        var person = await _personService.GetByIdAsync(id);
        if (person == null) return NotFound();
        return person;
    }

    [HttpPost]
    public async Task<IActionResult> Create(Person person)
    {
        await _personService.CreateAsync(person);
        return CreatedAtAction(nameof(GetById), new { id = person.Id }, person);
    }

    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> Update(string id, Person updatedPerson)
    {
        var person = await _personService.GetByIdAsync(id);
        if (person == null) return NotFound();

        await _personService.UpdateAsync(id, updatedPerson);
        return NoContent();
    }

    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {
        var person = await _personService.GetByIdAsync(id);
        if (person == null) return NotFound();

        await _personService.DeleteAsync(id);
        return NoContent();
    }
}
