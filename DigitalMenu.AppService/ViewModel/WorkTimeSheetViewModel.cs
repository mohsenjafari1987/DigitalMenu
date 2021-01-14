using System;
using System.Collections.Generic;
using System.Text;

namespace DigitalMenu.AppService.ViewModel
{
    public class WorkTimeViewModel
    {
        public string From { get; set; }
        public string To { get; set; }
        public string AvaiableFor { get; set; }
    }
    public class WorkTimeSheetViewModel
    {        
        public string Status { get; set; }
        public string Day { get; set; }
        public List<WorkTimeViewModel> WorkTimeViewModels { get; set; }
    }
}
