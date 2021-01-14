using DigitalMenu.AppService.Extention;
using DigitalMenu.AppService.Message;
using DigitalMenu.AppService.Request;
using DigitalMenu.AppService.Services;
using DigitalMenu.Model.Entities;
using DigitalMenu.Model.Enum;
using DigitalMenu.Repository.Context;
using DigitalMenu.Repository.Repository;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace DigitalMenu.NUnitTest
{
    class AppService_TimeWork
    {
        private DishService _dishService;
        private WorkTimeSheetService _workTimeService;

        [SetUp]
        public void Setup()
        {
            var mongoDBContext = new MongoDBContext("mongodb://127.0.0.1:27017", "DigitalMenu");
            var dishRepository = new DishRepository(mongoDBContext);
            var workTimeSheetRpository = new WorkTimeSheetRepository(mongoDBContext);
            _dishService = new DishService(dishRepository, workTimeSheetRpository);
            _workTimeService = new WorkTimeSheetService(workTimeSheetRpository);
        }

        [Test]
        public void GetMealType_ValidData()
        {
            List<WorkTimeSheet> workTimeSheets = _workTimeService.GetMockData();


            //2021-01-03 = Sunday but at this time (1:30 am) it should check yesterday (Saturday) working time.
            //Saturday working time: 20:00 to 2:00 AM set for Dinner
            DateTime dateTime = new DateTime(2021, 01, 03, 01, 30, 0);
            var response = dateTime.GetMealType(workTimeSheets);
            Assert.IsNotNull(response);
            Assert.AreEqual(response, MealType.Dinner);

            //2021-01-04 = Monday but at this time (1:30 am) it should consider yesterday (Sunday) working time.
            //Sunday working time: 20:00 to 2:00 AM  set for Lunch             
            dateTime = dateTime.AddDays(1);
            response = dateTime.GetMealType(workTimeSheets);
            Assert.IsNotNull(response);
            Assert.AreEqual(response, MealType.Lunch);


            //2021-01-05 = Tuesday but at this time (1:30 am) it should consider yesterday (Sunday) working time.
            //Monday working time: 20:00 to 23:00
            dateTime = dateTime.AddDays(1);
            response = dateTime.GetMealType(workTimeSheets);
            Assert.IsNull(response);

        }
    }
}
