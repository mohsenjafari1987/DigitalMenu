using DigitalMenu.Model.Entities;
using DigitalMenu.Repository.Context;
using DigitalMenu.Repository.GenericRepository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using MongoDB.Driver;
using DigitalMenu.Model.Enum;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;

namespace DigitalMenu.Repository.Repository
{
    public interface IDishRepository : IBaseRepository<Dish>
    {        
        public Task<IEnumerable<Dish>> GetMenuInTime(MealType mealType, Day day);
    }

    public class DishRepository : BaseRepository<Dish>, IDishRepository
    {
        public DishRepository(IMongoDBContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Dish>> GetMenuInTime(MealType mealType, Day day)
        {
            var data = await _dbCollection.FindAsync(r => r.IsDiactive == false 
                 && (r.MealType.Contains(mealType) || r.MealType.Contains(MealType.All))
                 && (r.Days.Contains(day) || r.Days.Contains(Day.AllDayOfWeek)));

            return data?.ToList();
        }

    }
}
