using DigitalMenu.Model.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace DigitalMenu.AppService.Request
{
    public class AddDishRequest
    {
        public string Title { get; set; }
        public int EstimatedTime { get; set; }
        public double Price { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }        
        public IList<int> MealType { get; set; }        
        public IList<int> Days { get; set; }
        public IDictionary<string, string> Ingredients { get; set; }
        public int AvailableCount { get; set; }
        public bool IsDiactive { get; set; }
    }
}
