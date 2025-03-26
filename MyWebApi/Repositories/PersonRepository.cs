using MyWebApi.Data;
using MyWebApi.Models;
using MongoDB.Driver;

namespace MyWebApi.Repositories
{
    public class PersonRepository
    {
        private readonly AppDbContext _context;

        public PersonRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Person>> GetAllAsync()
        {
            return await _context.Persons.Find(_ => true).ToListAsync();
        }

        public async Task<Person?> GetByIdAsync(string id)
        {
            return await _context.Persons.Find(p => p.Id == id).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(Person person)
        {
            await _context.Persons.InsertOneAsync(person);
        }

        public async Task UpdateAsync(string id, Person person)
        {
            await _context.Persons.ReplaceOneAsync(p => p.Id == id, person);
        }

        public async Task DeleteAsync(string id)
        {
            await _context.Persons.DeleteOneAsync(p => p.Id == id);
        }
    }
}
