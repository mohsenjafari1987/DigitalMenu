using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace DigitalMenu.Model.Base
{
    public abstract class BaseEntity : IEntity
    {
        [BsonId]
        public ObjectId Id { get; set; }

        public ObjectId GetId()
        {
            return Id;
        }
    }
}
