using DigitalMenu.AppService.Message;
using DigitalMenu.AppService.Response;
using DigitalMenu.AppService.ViewModel;
using DigitalMenu.Model.Entities;
using DigitalMenu.Repository.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace DigitalMenu.AppService.Services
{
    public class WorkTimeSheetService
    {
        private IWorkTimeSheetRepository _workTimeSheetRepository;

        public WorkTimeSheetService(IWorkTimeSheetRepository workTimeSheetRepository)
        {

            _workTimeSheetRepository = workTimeSheetRepository;
        }

        public async Task<BaseResponse> CreateSample()
        {
            BaseResponse result = new BaseResponse();

            List<WorkTimeSheet> workTimeSheets = GetMockData();
            try
            {
                await _workTimeSheetRepository.Create(workTimeSheets);
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

        public async Task<BaseResponse<WorkTimeSheetViewModel>> GetTodayAvailableTime()
        {
            BaseResponse<WorkTimeSheetViewModel> response = new BaseResponse<WorkTimeSheetViewModel>();
            try
            {
                var dbData = await _workTimeSheetRepository.GetAvailableTimeByDay(DateTime.Now.DayOfWeek);
                if (dbData == null)
                {
                    response.SetError(ErrorMessage.DatabaseEmpty);
                    return response;
                }

                response.Data = MapToViewModel(dbData);
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
        public async Task<BaseResponse<IList<WorkTimeSheetViewModel>>> GetAllAvailableTime()
        {
            BaseResponse<IList<WorkTimeSheetViewModel>> response = new BaseResponse<IList<WorkTimeSheetViewModel>>();
            try
            {
                var dbData = await _workTimeSheetRepository.Get();
                if (dbData == null)
                {
                    response.SetError(ErrorMessage.DatabaseEmpty);
                    return response;
                }

                response.Data = MapToViewModel(dbData);
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


        public List<WorkTimeSheet> GetMockData()
        {
            List<WorkTimeSheet> workTimeSheets = new List<WorkTimeSheet>();

            WorkTimeSheet monday = new WorkTimeSheet
            {
                Day = DayOfWeek.Monday,
                IsOpen = true,
                WorkTimes = new List<WorkTime>()
            };
            monday.WorkTimes.Add(new WorkTime { StartTime = 600, EndTime = 1100, MealType = Model.Enum.MealType.Breakfast });
            monday.WorkTimes.Add(new WorkTime { StartTime = 1200, EndTime = 1500, MealType = Model.Enum.MealType.Lunch });
            monday.WorkTimes.Add(new WorkTime { StartTime = 1700, EndTime = 1900, MealType = Model.Enum.MealType.Lunch });
            monday.WorkTimes.Add(new WorkTime { StartTime = 2000, EndTime = 2300, MealType = Model.Enum.MealType.Dinner });

            WorkTimeSheet tuesday = new WorkTimeSheet
            {
                Day = DayOfWeek.Tuesday,
                IsOpen = true,
                WorkTimes = new List<WorkTime>()
            };
            tuesday.WorkTimes.Add(new WorkTime { StartTime = 600, EndTime = 1100, MealType = Model.Enum.MealType.Breakfast });
            tuesday.WorkTimes.Add(new WorkTime { StartTime = 1200, EndTime = 1500, MealType = Model.Enum.MealType.Lunch });
            tuesday.WorkTimes.Add(new WorkTime { StartTime = 1700, EndTime = 1900, MealType = Model.Enum.MealType.Lunch });
            tuesday.WorkTimes.Add(new WorkTime { StartTime = 2000, EndTime = 2300, MealType = Model.Enum.MealType.Dinner });


            WorkTimeSheet wednesday = new WorkTimeSheet
            {
                Day = DayOfWeek.Wednesday,
                IsOpen = true,
                WorkTimes = new List<WorkTime>()
            };
            wednesday.WorkTimes.Add(new WorkTime { StartTime = 600, EndTime = 1100, MealType = Model.Enum.MealType.Breakfast });
            wednesday.WorkTimes.Add(new WorkTime { StartTime = 1200, EndTime = 1500, MealType = Model.Enum.MealType.Lunch });
            wednesday.WorkTimes.Add(new WorkTime { StartTime = 1700, EndTime = 1900, MealType = Model.Enum.MealType.Lunch });
            wednesday.WorkTimes.Add(new WorkTime { StartTime = 2000, EndTime = 2300, MealType = Model.Enum.MealType.Dinner });

            WorkTimeSheet thursday = new WorkTimeSheet
            {
                Day = DayOfWeek.Thursday,
                IsOpen = true,
                WorkTimes = new List<WorkTime>()
            };
            thursday.WorkTimes.Add(new WorkTime { StartTime = 600, EndTime = 1100, MealType = Model.Enum.MealType.Breakfast });
            thursday.WorkTimes.Add(new WorkTime { StartTime = 1200, EndTime = 1500, MealType = Model.Enum.MealType.Lunch });
            thursday.WorkTimes.Add(new WorkTime { StartTime = 1700, EndTime = 1900, MealType = Model.Enum.MealType.Lunch });
            thursday.WorkTimes.Add(new WorkTime { StartTime = 2000, EndTime = 2300, MealType = Model.Enum.MealType.Dinner });

            WorkTimeSheet friday = new WorkTimeSheet
            {
                Day = DayOfWeek.Friday,
                IsOpen = true,
                WorkTimes = new List<WorkTime>()
            };
            friday.WorkTimes.Add(new WorkTime { StartTime = 600, EndTime = 1100, MealType = Model.Enum.MealType.Breakfast });
            friday.WorkTimes.Add(new WorkTime { StartTime = 1200, EndTime = 1500, MealType = Model.Enum.MealType.Lunch });
            friday.WorkTimes.Add(new WorkTime { StartTime = 1700, EndTime = 1900, MealType = Model.Enum.MealType.Lunch });
            friday.WorkTimes.Add(new WorkTime { StartTime = 2000, EndTime = 2300, MealType = Model.Enum.MealType.Dinner });

            WorkTimeSheet saturday = new WorkTimeSheet
            {
                Day = DayOfWeek.Saturday,
                IsOpen = true,
                WorkTimes = new List<WorkTime>()
            };
            saturday.WorkTimes.Add(new WorkTime { StartTime = 600, EndTime = 1100, MealType = Model.Enum.MealType.Breakfast });
            saturday.WorkTimes.Add(new WorkTime { StartTime = 1200, EndTime = 1500, MealType = Model.Enum.MealType.Lunch });
            saturday.WorkTimes.Add(new WorkTime { StartTime = 1700, EndTime = 1900, MealType = Model.Enum.MealType.Lunch });
            saturday.WorkTimes.Add(new WorkTime { StartTime = 2000, EndTime = 200, MealType = Model.Enum.MealType.Dinner });


            WorkTimeSheet sunday = new WorkTimeSheet
            {
                Day = DayOfWeek.Sunday,
                IsOpen = true,
                WorkTimes = new List<WorkTime>()
            };
            sunday.WorkTimes.Add(new WorkTime { StartTime = 600, EndTime = 1100, MealType = Model.Enum.MealType.Breakfast });
            sunday.WorkTimes.Add(new WorkTime { StartTime = 1200, EndTime = 1500, MealType = Model.Enum.MealType.Lunch });
            sunday.WorkTimes.Add(new WorkTime { StartTime = 1700, EndTime = 1900, MealType = Model.Enum.MealType.Lunch });
            sunday.WorkTimes.Add(new WorkTime { StartTime = 2000, EndTime = 200, MealType = Model.Enum.MealType.Lunch });


            workTimeSheets.Add(monday);
            workTimeSheets.Add(tuesday);
            workTimeSheets.Add(wednesday);
            workTimeSheets.Add(thursday);
            workTimeSheets.Add(friday);
            workTimeSheets.Add(saturday);
            workTimeSheets.Add(sunday);

            return workTimeSheets;
        }
        private WorkTimeViewModel MapToViewMode(WorkTime workTime)
        {
            WorkTimeViewModel workTimeViewModel = new WorkTimeViewModel();
            var strTime = workTime.StartTime.ToString().PadLeft(4, '0');
            var endTime = workTime.EndTime.ToString().PadLeft(4, '0');
            workTimeViewModel.From = $"{strTime.Substring(0, 2)}:{strTime.Substring(2, 2)}";
            workTimeViewModel.To = $"{endTime.Substring(0, 2)}:{endTime.Substring(2, 2)}";
            workTimeViewModel.AvaiableFor = workTime.MealType.ToString();

            return workTimeViewModel;
        }
        private WorkTimeSheetViewModel MapToViewModel(WorkTimeSheet workTimeSheet)
        {
            WorkTimeSheetViewModel workTimeSheetViewModel = new WorkTimeSheetViewModel();

            workTimeSheetViewModel.Day = workTimeSheet.Day.ToString();
            if (!workTimeSheet.IsOpen)
            {
                workTimeSheetViewModel.Status = "Close";
                return workTimeSheetViewModel;
            }
            workTimeSheetViewModel.Status = "Open";
            workTimeSheetViewModel.WorkTimeViewModels = new List<WorkTimeViewModel>();
            foreach (var item in workTimeSheet.WorkTimes)
            {
                workTimeSheetViewModel.WorkTimeViewModels.Add(MapToViewMode(item));
            }

            return workTimeSheetViewModel;
        }
        private List<WorkTimeSheetViewModel> MapToViewModel(IEnumerable<WorkTimeSheet> workTimeSheets)
        {
            List<WorkTimeSheetViewModel> workTimeSheetViewModel = new List<WorkTimeSheetViewModel>();
            foreach (var item in workTimeSheets)
            {
                workTimeSheetViewModel.Add(MapToViewModel(item));
            }

            return workTimeSheetViewModel;
        }
    }
}
