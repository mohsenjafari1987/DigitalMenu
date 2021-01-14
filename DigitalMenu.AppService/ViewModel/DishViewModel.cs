using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace DigitalMenu.AppService.ViewModel
{
    public class DishViewModel
    {        
        public string Id { get; set; }
        public string Title { get; set; }
        public int EstimatedTime { get; set; }
        public double Price { get; set; }        
        public IDictionary<string, string> Ingredients { get; set; }       

    }
}
