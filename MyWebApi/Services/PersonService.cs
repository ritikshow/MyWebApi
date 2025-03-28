using MongoDB.Driver;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;
using MyWebApi.Configurations;
using MyWebApi.Models;

public class PersonService
{
    private readonly IMongoCollection<Person> _personCollection;
    private readonly EmailService _emailService;

    public PersonService(IOptions<MongoDbSettings> mongoSettings, EmailService emailService)
    {
        var mongoClient = new MongoClient(mongoSettings.Value.ConnectionString);
        var database = mongoClient.GetDatabase(mongoSettings.Value.DatabaseName);
        _personCollection = database.GetCollection<Person>(mongoSettings.Value.CollectionName);

        _emailService = emailService;
    }

    public async Task<List<Person>> GetAllAsync() => await _personCollection.Find(_ => true).ToListAsync();

    public async Task<Person> GetByIdAsync(string id) => await _personCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(Person person)
    {
        await _personCollection.InsertOneAsync(person);
        _emailService.SendEmail("New Booking!", $"A new person {person.Name},{person.Age} and phone number{person.PhoneNumber} has been added.");
    }

    public async Task UpdateAsync(string id, Person updatedPerson) =>
        await _personCollection.ReplaceOneAsync(x => x.Id == id, updatedPerson);

    public async Task DeleteAsync(string id) => await _personCollection.DeleteOneAsync(x => x.Id == id);
}
