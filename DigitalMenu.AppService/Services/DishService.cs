using DigitalMenu.AppService.ViewModel;
using DigitalMenu.AppService.Response;
using DigitalMenu.Repository.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using DigitalMenu.Model.Entities;
using DigitalMenu.Model.Enum;
using System.Threading.Tasks;
using DigitalMenu.AppService.Request;
using MongoDB.Bson;
using DigitalMenu.AppService.Extention;
using DigitalMenu.AppService.Message;

namespace DigitalMenu.AppService.Services
{
    public class DishService
    {
        private IDishRepository _dishRepository;
        private IWorkTimeSheetRepository _workTimeSheetRepository;

        public DishService(IDishRepository dishRepository, IWorkTimeSheetRepository workTimeSheetRepository)
        {
            _dishRepository = dishRepository;
            _workTimeSheetRepository = workTimeSheetRepository;
        }


        public async Task<BaseResponse<List<MenuCategoryViewModel>>> GetMenu()
        {
            var result = new BaseResponse<List<MenuCategoryViewModel>>();

            try
            {
                var dbData = await _dishRepository.Get();

                if (!dbData.Any())
                {
                    result.SetError(ErrorMessage.DatabaseEmpty);
                }
                else
                {
                    result.Data = new List<MenuCategoryViewModel>();
                    result.Data = MapToViewModel(dbData);
                }
            }
            catch (TimeoutException)
            {
                result.SetError(ErrorMessage.TimeoutError);
            }
            catch (Exception ex)
            {
                result.SetError(ex);
            }


            return result;
        }
        public async Task<BaseResponse<List<MenuCategoryViewModel>>> GetMenuInTime()
        {
            var result = new BaseResponse<List<MenuCategoryViewModel>>();

            try
            {
                var workTimeSheet = await _workTimeSheetRepository.Get();
                if (!CheckIsAvaiableResturant(workTimeSheet))
                {
                    result.SetError(ErrorMessage.RestaurantClosedToday);
                    return result;
                }

                MealType? MealType = DateTime.Now.GetMealType(workTimeSheet);
                if (MealType == null)
                {
                    result.SetError(ErrorMessage.OutOfService);
                    return result;
                }

                Day dayOfWeek = DateTime.Now.GetDay();

                var dbData = await _dishRepository.GetMenuInTime(MealType.Value, dayOfWeek);

                if (!dbData.Any())
                {
                    result.SetError(ErrorMessage.NoMenuAtThisTime);
                }
                else
                {
                    result.Data = new List<MenuCategoryViewModel>();
                    result.Data = MapToViewModel(dbData);
                }
            }
            catch (TimeoutException)
            {
                result.SetError(ErrorMessage.TimeoutError);
            }
            catch (Exception ex)
            {
                result.SetError(ex);
            }


            return result;
        }
        public async Task<BaseResponse<List<MenuCategoryViewModel>>> GetMenuByTime(int meal, int dayOfWeek)
        {
            var result = new BaseResponse<List<MenuCategoryViewModel>>();

            if (!Enum.IsDefined(typeof(MealType), meal))
            {
                result.SetError(ErrorMessage.MealTypeParameterNotValid);
                return result;
            }
            if (!Enum.IsDefined(typeof(Day), dayOfWeek))
            {
                result.SetError(ErrorMessage.DayParameterNotValid);
                return result;
            }

            try
            {
                var dbData = await _dishRepository.GetMenuInTime((MealType)meal, (Day)dayOfWeek);

                if (!dbData.Any())
                {
                    result.SetError(ErrorMessage.NoMenuAtThisTime);
                }
                else
                {
                    result.Data = new List<MenuCategoryViewModel>();
                    result.Data = MapToViewModel(dbData);
                }
            }
            catch (TimeoutException)
            {
                result.SetError(ErrorMessage.TimeoutError);
            }
            catch (Exception ex)
            {
                result.SetError(ex);
            }

            return result;
        }


        public async Task<BaseResponse> ActiveDish(string id)
        {
            return await ActiveDeactive(id, false);
        }
        public async Task<BaseResponse> DeactiveDish(string id)
        {
            return await ActiveDeactive(id, true);
        }
        private async Task<BaseResponse> ActiveDeactive(string id, bool deactiveStatus)
        {
            BaseResponse response = new BaseResponse();

            try
            {
                var dish = await _dishRepository.Get(ObjectId.Parse(id));

                if (dish == null)
                {
                    response.SetError(ErrorMessage.RecordNotFound);
                    return response;
                }

                dish.IsDiactive = deactiveStatus;
                await _dishRepository.Update(dish);
            }
            catch (FormatException)
            {
                response.SetError(ErrorMessage.FormatError);
            }
            catch (TimeoutException)
            {
                response.SetError(ErrorMessage.TimeoutError);
            }
            catch (Exception ex)
            {
                response.SetError(ex);
            }

            return response;
        }


        public async Task<AddDishResponse> AddDish(AddDishRequest addDishRequest)
        {
            AddDishResponse addDishResponse = addDishRequest.IsValid();

            if (addDishResponse.Success)
            {
                try
                {
                    Dish dish = MapToDataBaseModel(addDishRequest);
                    await _dishRepository.Create(dish);
                    addDishResponse.Id = dish.Id.ToString();
                }
                catch (TimeoutException)
                {
                    addDishResponse.SetError(ErrorMessage.TimeoutError);
                }
                catch (Exception ex)
                {
                    addDishResponse.SetError(ex);
                }
            }

            return addDishResponse;
        }
        public async Task<BaseResponse> Update(string id, AddDishRequest addDishRequest)
        {

            BaseResponse addDishResponse = addDishRequest.IsValid();

            if (addDishResponse.Success)
            {
                try
                {
                    var dbDish = await _dishRepository.Get(ObjectId.Parse(id));

                    if (dbDish == null)
                    {
                        addDishResponse.SetError(ErrorMessage.RecordNotFound);
                        return addDishResponse;
                    }

                    Dish newDish = MapToDataBaseModel(addDishRequest);
                    newDish.Id = dbDish.Id;
                    await _dishRepository.Update(newDish);
                }

                catch (TimeoutException)
                {
                    addDishResponse.SetError(ErrorMessage.TimeoutError);
                }
                catch (FormatException)
                {
                    addDishResponse.SetError(ErrorMessage.FormatError);
                }
                catch (Exception ex)
                {
                    addDishResponse.SetError(ex);
                }
            }

            return addDishResponse;
        }
        public async Task<BaseResponse> DeleteDish(string id)
        {
            BaseResponse response = new BaseResponse();
            try
            {
                var dish = await _dishRepository.Get(ObjectId.Parse(id));
                if (dish == null)
                {
                    response.SetError(ErrorMessage.RecordNotFound);
                    return response;
                }
                await _dishRepository.Delete(dish.Id);
            }
            catch (TimeoutException)
            {
                response.SetError(ErrorMessage.TimeoutError);
            }
            catch (FormatException)
            {
                response.SetError(ErrorMessage.FormatError);
            }
            catch (Exception ex)
            {
                response.SetError(ex);

            }
            return response;
        }


        public async Task<BaseResponse> CreateSample()
        {
            BaseResponse result = new BaseResponse();

            List<Dish> dishes = new List<Dish>();

            Dish peperoni = new Dish
            {
                Title = "Peperoni".ToTitleForStore(),
                AvailableCount = 60,
                Category = "Main Course".ToTitleForStore(),
                SubCategory = "Pizza".ToTitleForStore(),
                EstimatedTime = 40,
                IsDiactive = false,
                Price = 10,
                MealType = new List<MealType>(),
                Days = new List<Day>(),
                Ingredients = new Dictionary<string, string>(),
            };
            peperoni.MealType.Add(MealType.Breakfast);
            peperoni.MealType.Add(MealType.Lunch);
            peperoni.MealType.Add(MealType.Dinner);

            peperoni.Days.Add(Day.Monday);

            peperoni.Ingredients.Add(new KeyValuePair<string, string>("Sausage", "100g"));
            peperoni.Ingredients.Add(new KeyValuePair<string, string>("Cheese", "150g"));

            Dish mix = new Dish
            {
                Title = "Milano",
                AvailableCount = 60,
                Category = "Main course".ToTitleForStore(),
                SubCategory = "Pizza".ToTitleForStore(),
                EstimatedTime = 30,
                IsDiactive = false,
                Price = 10,
                MealType = new List<MealType>(),
                Days = new List<Day>(),
                Ingredients = new Dictionary<string, string>(),
            };
            mix.MealType.Add(MealType.All);

            mix.Days.Add(Day.AllDayOfWeek);

            mix.Ingredients.Add(new KeyValuePair<string, string>("Sausage", "100g"));
            mix.Ingredients.Add(new KeyValuePair<string, string>("Cheese", "150g"));

            Dish Cheeseburger = new Dish
            {
                Title = "Cheese Burger",
                AvailableCount = 60,
                Category = " main course".ToTitleForStore(),
                SubCategory = "Hamburger".ToTitleForStore(),
                EstimatedTime = 30,
                IsDiactive = false,
                Price = 10,
                MealType = new List<MealType>(),
                Days = new List<Day>(),
                Ingredients = new Dictionary<string, string>(),
            };
            Cheeseburger.MealType.Add(MealType.Lunch);
            Cheeseburger.MealType.Add(MealType.Dinner);

            Cheeseburger.Days.Add(Day.AllDayOfWeek);

            Cheeseburger.Ingredients.Add(new KeyValuePair<string, string>("Hamburger", "100g"));
            Cheeseburger.Ingredients.Add(new KeyValuePair<string, string>("Lettuce", "150g"));

            Dish doubleBurger = new Dish
            {
                Title = "Double Burger".ToTitleForStore(),
                AvailableCount = 60,
                Category = "Main Course".ToTitleForStore(),
                SubCategory = "Hamburger".ToTitleForStore(),
                EstimatedTime = 30,
                IsDiactive = false,
                Price = 10,
                MealType = new List<MealType>(),
                Days = new List<Day>(),
                Ingredients = new Dictionary<string, string>(),

            };
            doubleBurger.MealType.Add(MealType.Lunch);
            doubleBurger.MealType.Add(MealType.Dinner);

            doubleBurger.Days.Add(Day.AllDayOfWeek);

            doubleBurger.Ingredients.Add(new KeyValuePair<string, string>("Hamburger", "300g"));
            doubleBurger.Ingredients.Add(new KeyValuePair<string, string>("Cheese", "150g"));

            Dish ItalianIceCream = new Dish
            {
                Title = "   italian Ice Cream".ToTitleForStore(),
                AvailableCount = 60,
                Category = "Dessert   ".ToTitleForStore(),
                SubCategory = "Ice Cream".ToTitleForStore(),
                EstimatedTime = 30,
                IsDiactive = false,
                Price = 10,
                MealType = new List<MealType>(),
                Days = new List<Day>(),
                Ingredients = new Dictionary<string, string>(),
            };
            ItalianIceCream.MealType.Add(MealType.Lunch);
            ItalianIceCream.MealType.Add(MealType.Dinner);

            ItalianIceCream.Days.Add(Day.AllDayOfWeek);

            Dish Salad = new Dish
            {
                Title = "   salad sezar".ToTitleForStore(),
                AvailableCount = 60,
                Category = "Starter   ".ToTitleForStore(),
                SubCategory = "Salad".ToTitleForStore(),
                EstimatedTime = 30,
                IsDiactive = false,
                Price = 10,
                MealType = new List<MealType>(),
                Days = new List<Day>(),
                Ingredients = new Dictionary<string, string>(),
            };
            Salad.MealType.Add(MealType.Lunch);
            Salad.MealType.Add(MealType.Dinner);

            Salad.Days.Add(Day.AllDayOfWeek);



            dishes.Add(peperoni);
            dishes.Add(mix);
            dishes.Add(Cheeseburger);
            dishes.Add(doubleBurger);
            dishes.Add(ItalianIceCream);
            dishes.Add(Salad);

            try
            {
                await _dishRepository.Create(dishes);

            }
            catch (TimeoutException)
            {
                result.SetError(ErrorMessage.TimeoutError);
            }
            catch (Exception ex)
            {
                result.SetError(ex);
            }

            return result;
        }

        private List<MenuCategoryViewModel> MapToViewModel(IEnumerable<Dish> dishes)
        {
            var result = new List<MenuCategoryViewModel>();

            foreach (var item in dishes.GroupBy(b => new { b.Category, b.SubCategory }))
            {
                MenuCategoryViewModel menuCategoryViewModel = result.FirstOrDefault(r => r.Title.ToTitleForStore() == item.Key.Category);

                if (menuCategoryViewModel == null)
                {
                    menuCategoryViewModel = new MenuCategoryViewModel { Title = item.Key.Category.ToTitleForView() };
                    result.Add(menuCategoryViewModel);
                }

                menuCategoryViewModel.SubCategories.Add(new SubCategory
                {
                    Title = item.Key.SubCategory.ToTitleForView(),
                    DishViewModels = item.Select(dish => new DishViewModel
                    {
                        Title = dish.Title.ToTitleForView(),
                        EstimatedTime = dish.EstimatedTime,
                        Ingredients = dish?.Ingredients,
                        Id = dish.Id.ToString(),
                        Price = dish.Price,
                        Description = dish.Description
                    }).ToList()
                });
            }

            return result;
        }

        private Dish MapToDataBaseModel(AddDishRequest addDishRequest)
        {
            Dish dish = new Dish
            {
                Title = addDishRequest.Title.ToTitleForStore(),
                AvailableCount = addDishRequest.AvailableCount,
                MealType = addDishRequest.MealType.Select(r => (MealType)r).ToList(),
                Days = addDishRequest.Days.Select(r => (Day)r).ToList(),
                Category = addDishRequest.Category.ToTitleForStore(),
                SubCategory = addDishRequest.SubCategory.ToTitleForStore(),
                EstimatedTime = addDishRequest.EstimatedTime,
                Ingredients = addDishRequest.Ingredients,
                IsDiactive = addDishRequest.IsDiactive,
                Price = addDishRequest.Price,
                Description = addDishRequest.Description
            };

            return dish;
        }

        private bool CheckIsAvaiableResturant(IEnumerable<WorkTimeSheet> workTimeSheets)
        {
            return workTimeSheets.Any(r => r.Day == DateTime.Now.DayOfWeek && r.IsOpen);
        }

    }
}
