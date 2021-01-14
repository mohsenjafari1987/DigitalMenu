using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace DigitalMenu.Model.Base
{
    public interface IEntity
    {
        public ObjectId Id { get; set; }
        public ObjectId GetId();
    }
}
