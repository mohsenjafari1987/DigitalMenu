using DigitalMenu.Model.Base;
using DigitalMenu.Model.Enum;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace DigitalMenu.Model.Entities
{
    public class Dish : BaseEntity
    {
        public string Title { get; set; }
        public int EstimatedTime { get; set; }
        public double Price { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public IList<MealType> MealType { get; set; }
        public IList<Day> Days { get; set; }
        public IDictionary<string, string> Ingredients { get; set; }
        public int AvailableCount { get; set; }
        public bool IsDiactive { get; set; }
        public string Description { get; set; }


        public Dish()
        {
            Id = ObjectId.GenerateNewId();
        }
    }
}
