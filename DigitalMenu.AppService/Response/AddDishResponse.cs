using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace DigitalMenu.AppService.Response
{
    public class AddDishResponse : BaseResponse
    {
        public string Id { get; set; }

        
    }
}
