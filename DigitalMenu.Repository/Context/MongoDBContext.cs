using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace DigitalMenu.Repository.Context
{
    public class MongoDBContext : IMongoDBContext
    {
        private IMongoDatabase _db { get; set; }
        private MongoClient _mongoClient { get; set; }
        public IClientSessionHandle Session { get; set; }
        public MongoDBContext(string connectionString, string databaseName)
        {
            //BsonDefaults.GuidRepresentation = GuidRepresentation.CSharpLegacy;
            _mongoClient = new MongoClient(connectionString);
            _db = _mongoClient.GetDatabase(databaseName);
        }
        public IMongoCollection<T> GetCollection<T>(string name)
        {
            return _db.GetCollection<T>(name);
        }
    }
}
