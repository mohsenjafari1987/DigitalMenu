using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace DigitalMenu.Repository.Context
{
    public interface IMongoDBContext
    {
        IMongoCollection<T> GetCollection<T>(string name);
    }
}
