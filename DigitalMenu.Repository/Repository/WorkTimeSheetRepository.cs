using DigitalMenu.Model.Entities;
using DigitalMenu.Repository.Context;
using DigitalMenu.Repository.GenericRepository;
using System;
using System.Threading.Tasks;
using System.Linq;
using MongoDB.Driver;


namespace DigitalMenu.Repository.Repository
{
    public interface IWorkTimeSheetRepository : IBaseRepository<WorkTimeSheet>
    {
        Task<WorkTimeSheet> GetAvailableTimeByDay(DayOfWeek dayOfWeek);
    }

    public class WorkTimeSheetRepository : BaseRepository<WorkTimeSheet>, IWorkTimeSheetRepository
    {
        public WorkTimeSheetRepository(IMongoDBContext context) : base(context)
        {
        }
        public async Task<WorkTimeSheet> GetAvailableTimeByDay(DayOfWeek dayOfWeek)
        {
            var data = await _dbCollection.FindAsync(r => r.Day == dayOfWeek);

            return data?.FirstOrDefault();
        }
    }
}
