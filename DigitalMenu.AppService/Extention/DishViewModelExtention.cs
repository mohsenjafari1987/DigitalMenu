using DigitalMenu.AppService.Message;
using DigitalMenu.AppService.Request;
using DigitalMenu.AppService.Response;
using DigitalMenu.AppService.ViewModel;
using DigitalMenu.Model.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace DigitalMenu.AppService.Extention
{
    public static class DishViewModelExtention
    {
        public static AddDishResponse IsValid(this AddDishRequest addDishRequest)
        {
            AddDishResponse addDishResponse = new AddDishResponse();
            if (string.IsNullOrEmpty(addDishRequest.Title))
            {
                addDishResponse.SetError(ErrorMessage.TitleParameterNotValid);
                return addDishResponse;
            }
            if (string.IsNullOrEmpty(addDishRequest.Category))
            {
                addDishResponse.SetError(ErrorMessage.CategoryParameterNotValid);
                return addDishResponse;
            }
            if (string.IsNullOrEmpty(addDishRequest.SubCategory))
            {
                addDishResponse.SetError(ErrorMessage.SubCategoryParameterNotValid);
                return addDishResponse;
            }
            if (addDishRequest.Price <= 0)
            {
                addDishResponse.SetError(ErrorMessage.PriceParameterNotValid);
                return addDishResponse;
            }
            if (addDishRequest.EstimatedTime <= 0)
            {
                addDishResponse.SetError(ErrorMessage.EstimatedTimeParameterNotValid);
                return addDishResponse;
            }

            foreach (var item in addDishRequest.MealType)
            {
                if (!Enum.IsDefined(typeof(MealType), item))
                {
                    addDishResponse.SetError(ErrorMessage.MealTypeParameterNotValid);
                    return addDishResponse;
                }
            }
            foreach (var item in addDishRequest.Days)
            {
                if (!Enum.IsDefined(typeof(Day), item))
                {
                    addDishResponse.SetError(ErrorMessage.DayParameterNotValid);
                    return addDishResponse;
                }
            }

            return addDishResponse;
        }
    }
}
