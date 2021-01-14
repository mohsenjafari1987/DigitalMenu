using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace DigitalMenu.Model.Enum
{
    public enum MealType
    {
        All = 1,
        Breakfast = 2,
        Lunch = 3,
        Dinner = 4
    }
}
