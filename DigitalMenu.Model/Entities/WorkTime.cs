using DigitalMenu.Model.Base;
using DigitalMenu.Model.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace DigitalMenu.Model.Entities
{
    public class WorkTime
    {
        public int StartTime { get; set; }
        public int EndTime { get; set; }
        public MealType MealType { get; set; }
    }
    public class WorkTimeSheet: BaseEntity
    {        
        public DayOfWeek Day{ get; set; }
        public bool IsOpen { get; set; }
        public IList<WorkTime> WorkTimes { get; set; }
    }
}
