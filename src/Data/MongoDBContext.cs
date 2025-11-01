using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TallerDos.ProductsService.src.models;

namespace TallerDos.ProductsService.src.Data
{
    public class MongoDbSettings
    {
        public string ConnectionString { get; set; } = string.Empty;
        public string DatabaseName { get; set; } = string.Empty;
    }
    public class MongoDBContext
    {        
        private readonly IMongoDatabase _database;
        public IMongoCollection<Product> Products => _database.GetCollection<Product>("products");

        public MongoDBContext(IOptions<MongoDbSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            _database = client.GetDatabase(settings.Value.DatabaseName);
        }
    }
    
}