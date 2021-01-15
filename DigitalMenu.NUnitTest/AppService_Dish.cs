using DigitalMenu.AppService.Extention;
using DigitalMenu.AppService.Message;
using DigitalMenu.AppService.Request;
using DigitalMenu.AppService.Services;
using DigitalMenu.Repository.Context;
using DigitalMenu.Repository.Repository;
using MongoDB.Bson;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace DigitalMenu.NUnitTest
{

    class AppService_Dish
    {
        private DishService _dishService;

        [SetUp]
        public void Setup()
        {
            var mongoDBContext = new MongoDBContext("mongodb://127.0.0.1:27017", "DigitalMenu");
            var dishRepository = new DishRepository(mongoDBContext);
            var workTimeSheetRpository = new WorkTimeSheetRepository(mongoDBContext);
            _dishService = new DishService(dishRepository, workTimeSheetRpository);
        }

        [Test]
        public void DishModel_Validate()
        {

            AddDishRequest addDishRequest = new AddDishRequest();

            //Test Empty Model
            var validateResponse = addDishRequest.IsValid();
            Assert.AreEqual(false, validateResponse.Success);

            addDishRequest = new AddDishRequest
            {
                Title = "peperoni",
                AvailableCount = 60,
                Category = "Main Course",
                SubCategory = "Pizza",
                EstimatedTime = 40,
                IsDiactive = false,
                Price = 10,
                MealType = new List<int>(),
                Days = new List<int>(),
                Ingredients = new Dictionary<string, string>(),
            };
            addDishRequest.MealType.Add(1);
            addDishRequest.MealType.Add(2);

            addDishRequest.Days.Add(1);

            addDishRequest.Ingredients.Add(new KeyValuePair<string, string>("Sausage", "100g"));
            addDishRequest.Ingredients.Add(new KeyValuePair<string, string>("Cheese", "150g"));

            //Test Wrong MealType
            addDishRequest.MealType.Add(0); //0 is not valid
            validateResponse = addDishRequest.IsValid();
            Assert.AreEqual(false, validateResponse.Success);
            Assert.AreEqual(ErrorMessage.MealTypeParameterNotValid, validateResponse.Message);
            addDishRequest.MealType.Remove(0);

            //Test Wrong Day
            addDishRequest.Days.Add(0);// 0 is not valid            
            validateResponse = addDishRequest.IsValid();
            Assert.AreEqual(false, validateResponse.Success);
            Assert.AreEqual(ErrorMessage.DayParameterNotValid, validateResponse.Message);
            addDishRequest.Days.Remove(0);

            //Test wrong Category
            addDishRequest.Category = string.Empty;
            validateResponse = addDishRequest.IsValid();
            Assert.AreEqual(false, validateResponse.Success);
            Assert.AreEqual(ErrorMessage.CategoryParameterNotValid, validateResponse.Message);
            addDishRequest.Category = "MainCourse";

            //Test wrong SubCategory
            addDishRequest.SubCategory = string.Empty;
            validateResponse = addDishRequest.IsValid();
            Assert.AreEqual(false, validateResponse.Success);
            Assert.AreEqual(ErrorMessage.SubCategoryParameterNotValid, validateResponse.Message);
            addDishRequest.SubCategory = "Pizza";

            //Test wrong Price
            addDishRequest.Price = default;
            validateResponse = addDishRequest.IsValid();
            Assert.AreEqual(false, validateResponse.Success);
            Assert.AreEqual(ErrorMessage.PriceParameterNotValid, validateResponse.Message);
            addDishRequest.Price = 3;

            //Test wrong EstimatedTime
            addDishRequest.EstimatedTime = default;
            validateResponse = addDishRequest.IsValid();
            Assert.AreEqual(false, validateResponse.Success);
            Assert.AreEqual(ErrorMessage.EstimatedTimeParameterNotValid, validateResponse.Message);
            addDishRequest.Price = 30;

            //Test wrong Title
            addDishRequest.Title = string.Empty;
            validateResponse = addDishRequest.IsValid();
            Assert.AreEqual(false, validateResponse.Success);
            Assert.AreEqual(ErrorMessage.TitleParameterNotValid, validateResponse.Message);

        }

        [Test]
        public void TextToTitle()
        {
            string input = " main Course ";
            string expected = "Main_Course";
            string actual = input.ToTitleForStore();

            Assert.AreEqual(expected, actual);

            input = "Main_Course";
            expected = "Main Course";
            actual = input.ToTitleForView();
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetMenuByTime_CheckWrongData()
        {
            var actual = _dishService.GetMenuByTime(1, 0).Result;
            var expectedResult = false;
            var expectedMessage = ErrorMessage.DayParameterNotValid;
            Assert.AreEqual(expectedResult, actual.Success);
            Assert.AreEqual(expectedMessage, actual.Message);

            actual = _dishService.GetMenuByTime(0, 1).Result;
            expectedResult = false;
            expectedMessage = ErrorMessage.MealTypeParameterNotValid;
            Assert.AreEqual(expectedResult, actual.Success);
            Assert.AreEqual(expectedMessage, actual.Message);
        }

        [Test]
        public void ActiveDeavtiveCheck_WrongFormatId()
        {
            var actual = _dishService.ActiveDish("WrongData").Result;
            Assert.IsFalse(actual.Success);
            Assert.AreEqual(actual.Message, ErrorMessage.FormatError);


            actual = _dishService.ActiveDish(ObjectId.GenerateNewId().ToString()).Result;
            Assert.IsFalse(actual.Success);
            Assert.AreEqual(actual.Message, ErrorMessage.RecordNotFound);
        }

        [Test]
        public void UpdateCheck_WrongData()
        {
            var actual = _dishService.Update(ObjectId.GenerateNewId().ToString(), new AddDishRequest()).Result;
            Assert.IsFalse(actual.Success);                       
        }        

        [Test]
        public void Database_NotFound()
        {
            var mongoDBContext = new MongoDBContext("mongodb://127.0.0.1:27017", "_DigitalMenu2");
            var dishRepository = new DishRepository(mongoDBContext);
            var workTimeSheetRpository = new WorkTimeSheetRepository(mongoDBContext);
            var dishService = new DishService(dishRepository, workTimeSheetRpository);

            var actual = dishService.GetMenu().Result;
            Assert.IsFalse(actual.Success);
            Assert.AreEqual(actual.Message, ErrorMessage.DatabaseEmpty);
        }

        [Test]
        public void DeleteCheck_WrongFormatId()
        {
            var actual = _dishService.DeleteDish("WrongData").Result;
            Assert.IsFalse(actual.Success);
            Assert.AreEqual(actual.Message, ErrorMessage.FormatError);


            actual = _dishService.ActiveDish(ObjectId.GenerateNewId().ToString()).Result;
            Assert.IsFalse(actual.Success);
            Assert.AreEqual(actual.Message, ErrorMessage.RecordNotFound);
        }
    }
}