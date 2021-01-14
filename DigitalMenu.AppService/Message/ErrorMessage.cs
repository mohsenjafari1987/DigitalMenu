
namespace DigitalMenu.AppService.Message
{
    public static class ErrorMessage
    {
        public const string TimeoutError = "Timeout error, please try again or check your connection string";
        public const string FormatError = "Id parameter is not valid";
        public const string RecordNotFound = "Record does not exist in the database";
        public const string DatabaseEmpty = "Database is empty";
        public const string OutOfService = "The restaurant at this time is out of service, try for another time";
        public const string RestaurantClosedToday = "The restaurant is closed today";
        public const string NoMenuAtThisTime = "There are not any dishes for your search parameter";
        public const string TitleParameterNotValid = "Title parameter is not valid";
        public const string CategoryParameterNotValid = "Category parameter is not valid";
        public const string SubCategoryParameterNotValid = "SubCategory parameter is not valid";
        public const string PriceParameterNotValid = "Price parameter is not valid";
        public const string EstimatedTimeParameterNotValid = "Estimated time paramater is not valid";
        public const string MealTypeParameterNotValid = "AvaialbeInDay parameter is not valid, set a list of number(1-3)";
        public const string DayParameterNotValid = "Day parameter is not valid, set a list of number(1=AllDays or 2=Monday-8=Sunday)";
    }
}
