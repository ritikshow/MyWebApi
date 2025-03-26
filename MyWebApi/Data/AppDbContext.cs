using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MyWebApi.Configurations;
using MyWebApi.Models;

namespace MyWebApi.Data
{
    public class AppDbContext
    {
        private readonly IMongoDatabase _database;

        public AppDbContext(IOptions<MongoDbSettings> mongoDbSettings)
        {
            var client = new MongoClient(mongoDbSettings.Value.ConnectionString);
            _database = client.GetDatabase(mongoDbSettings.Value.DatabaseName);
        }

        public IMongoCollection<Person> Persons => _database.GetCollection<Person>("Persons");
    }
}
