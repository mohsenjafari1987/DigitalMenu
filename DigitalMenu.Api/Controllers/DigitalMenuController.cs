using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DigitalMenu.AppService.Request;
using DigitalMenu.AppService.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DigitalMenu.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DigitalMenuController : ControllerBase
    {
        private readonly DishService _dishService;
        private readonly WorkTimeSheetService _workTimeSheetService;

        public DigitalMenuController(DishService dishService, WorkTimeSheetService workTimeSheetService)
        {
            _dishService = dishService;
            _workTimeSheetService = workTimeSheetService;
        }

        /// <summary>
        /// fiojsdofj sosodj fosd foisd foisdhf ihdf
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("[controller]/[action]")]
        public async Task<IActionResult> CreateSampleMenu()
        {
            var result = await _dishService.CreateSample();

            return Ok(result);
        }

        [HttpPost]
        [Route("[controller]/[action]")]
        public async Task<IActionResult> CreateSampleWorkTimeSheet()
        {
            var result = await _workTimeSheetService.CreateSample();

            return Ok(result);
        }

        [HttpGet]
        [Route("[controller]/[action]")]
        public async Task<IActionResult> GetTodayAvailableTime()
        {
            var result = await _workTimeSheetService.GetTodayAvailableTime();

            return Ok(result);
        }

        [HttpGet]
        [Route("[controller]/[action]")]
        public async Task<IActionResult> GetAllAvailableTime()
        {
            var result = await _workTimeSheetService.GetAllAvailableTime();

            return Ok(result);
        }

        [HttpGet]
        [Route("[controller]/[action]")]
        public async Task<IActionResult> GetAllMenu()
        {

            var result = await _dishService.GetMenu();

            return Ok(result);
        }


        [HttpGet]
        [Route("[controller]/[action]")]
        public async Task<IActionResult> GetMenuInTime()
        {
            var result = await _dishService.GetMenuInTime();
            return Ok(result);
        }

        [HttpGet]
        [Route("[controller]/[action]")]
        public async Task<IActionResult> GetMenuByTime(int meal, int dayOfWeek)
        {
            var result = await _dishService.GetMenuByTime(meal, dayOfWeek);
            return Ok(result);
        }

        [HttpPost]
        [Route("[controller]/[action]")]
        public async Task<IActionResult> AddDish(AddDishRequest addDishRequest)
        {

            var result = await _dishService.AddDish(addDishRequest);

            return Ok(result);
        }

        [HttpPost]
        [Route("[controller]/[action]/{id}")]
        public async Task<IActionResult> UpdateDish(string id, AddDishRequest addDishRequest)
        {

            var result = await _dishService.Update(id, addDishRequest);

            return Ok(result);
        }

        [HttpPost]
        [Route("[controller]/[action]/{id}")]
        public async Task<IActionResult> DeactiveDish(string id)
        {
            var result = await _dishService.DeactiveDish(id);
            return Ok(result);
        }

        [HttpPost]
        [Route("[controller]/[action]/{id}")]
        public async Task<IActionResult> ActiveDish(string id)
        {
            var result = await _dishService.ActiveDish(id);
            return Ok(result);
        }

        [HttpDelete]
        [Route("[controller]/[action]/{id}")]
        public async Task<IActionResult> DeleteDish(string id)
        {
            var result = await _dishService.DeleteDish(id);
            return Ok(result);
        }

    }
}
