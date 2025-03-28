using MongoDB.Driver;
using MyWebApi.Configurations;
using MyWebApi.Models;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyWebApi.Repositories
{
    public class PersonRepository
    {
        private readonly IMongoCollection<Person> _personsCollection;

        public PersonRepository(IOptions<MongoDbSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            var database = client.GetDatabase(settings.Value.DatabaseName);
            _personsCollection = database.GetCollection<Person>(settings.Value.CollectionName);
        }

        public async Task<List<Person>> GetAllPersonsAsync()
        {
            return await _personsCollection.Find(_ => true).ToListAsync();
        }

        public async Task<Person> GetPersonByIdAsync(string id)
        {
            return await _personsCollection.Find(p => p.Id == id).FirstOrDefaultAsync();
        }

        public async Task CreatePersonAsync(Person person)
        {
            await _personsCollection.InsertOneAsync(person);
        }

        public async Task<bool> UpdatePersonAsync(Person person)
        {
            var result = await _personsCollection.ReplaceOneAsync(p => p.Id == person.Id, person);
            return result.ModifiedCount > 0;
        }

        public async Task<bool> DeletePersonAsync(string id)
        {
            var result = await _personsCollection.DeleteOneAsync(p => p.Id == id);
            return result.DeletedCount > 0;
        }
    }
}
