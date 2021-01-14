using DigitalMenu.Model.Entities;
using DigitalMenu.Model.Enum;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Globalization;

namespace DigitalMenu.AppService.Extention
{
    public static class EnumExtention
    {
        private static TimeSpan ToTimeSpan(this int time)
        {
            try
            {
                var date = DateTime.ParseExact(time.ToString().PadLeft(4, '0'), "HHmm", CultureInfo.InvariantCulture);
                return date.TimeOfDay;
            }
            catch (Exception ex)
            {
                throw new Exception("Time Format is not valid");
            }
        }
        private static DateTime ToDateTime(this int time, DateTime dateTime)
        {
            try
            {
                var timeSpan = time.ToTimeSpan();
                return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day) + timeSpan;
            }
            catch
            {
                throw new Exception("Time Format is not valid");
            }
        }
        private static MealType? GetMealType(this DateTime dateTime, WorkTimeSheet workTimeSheet, bool checkOneDayBefor = false)
        {
            foreach (var item in workTimeSheet?.WorkTimes)
            {
                DateTime fromTime = item.StartTime.ToDateTime(dateTime);
                DateTime toTime = item.EndTime.ToDateTime(dateTime);
                if (checkOneDayBefor && fromTime > toTime)
                {
                    fromTime = fromTime.AddDays(-1);
                }


                if (dateTime >= fromTime && dateTime <= toTime)
                {
                    return item.MealType;
                }
            }

            return null;
        }

        public static MealType? GetMealType(this DateTime dateTime, IEnumerable<WorkTimeSheet> workTimeSheets)
        {
            var currentDateWorkTimes = workTimeSheets.FirstOrDefault(r => r.Day == dateTime.DayOfWeek);
            var availableDayToday = dateTime.GetMealType(currentDateWorkTimes);

            /*Imagine a restaurant works on Monday from 18:00 To 02:00 AM and
            - If today is Tuesday and at 1:00 AM we should consider yesterday as a current day (Monday) 
            - because Tuesday at 1:00 is set for Monday working time*/
            if (availableDayToday == null)
            {
                var earliestStartTime = currentDateWorkTimes.WorkTimes.Min(r => r.StartTime);
                var strTime = earliestStartTime.ToTimeSpan();

                if (dateTime.TimeOfDay < strTime)
                {
                    //get yesterday work time sheet
                    currentDateWorkTimes = workTimeSheets.FirstOrDefault(r => r.Day == dateTime.AddDays(-1).DayOfWeek);
                    availableDayToday = dateTime.GetMealType(currentDateWorkTimes, true); //check yesterday time work
                }
            }


            return availableDayToday;
        }
        public static Day GetDay(this DateTime dateTime)
        {
            Day Day = dateTime.DayOfWeek switch
            {
                DayOfWeek.Monday => Day.Monday,
                DayOfWeek.Tuesday => Day.Tuesday,
                DayOfWeek.Wednesday => Day.Wednesday,
                DayOfWeek.Thursday => Day.Thursday,
                DayOfWeek.Friday => Day.Friday,
                DayOfWeek.Saturday => Day.Saturday,
                DayOfWeek.Sunday => Day.Sunday,

            };
            return Day;
        }

    }
}
