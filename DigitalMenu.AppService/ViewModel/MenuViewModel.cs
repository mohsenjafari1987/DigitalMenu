using System;
using System.Collections.Generic;
using System.Text;

namespace DigitalMenu.AppService.ViewModel
{
    public class SubCategory
    {
        public SubCategory()
        {
            DishViewModels = new List<DishViewModel>();
        }
        public string Title { get; set; }
        public List<DishViewModel> DishViewModels { get; set; }
    }
    public class MenuCategoryViewModel
    {
        public string Title { get; set; }
        public List<SubCategory> SubCategories { get; set; }
        public MenuCategoryViewModel()
        {
            SubCategories = new List<SubCategory>();
        }
    }
    
}
