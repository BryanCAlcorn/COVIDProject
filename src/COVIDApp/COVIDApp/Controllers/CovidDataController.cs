using System;
using System.Threading.Tasks;
using COVIDData;
using Microsoft.AspNetCore.Mvc;

namespace COVIDApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CovidDataController : ControllerBase
    {
        private readonly ICovidDataRepository _dataRepository;

        public CovidDataController(ICovidDataRepository covidDataRepository)
        {
            _dataRepository = covidDataRepository;
        }

        [HttpGet]
        public async Task<IActionResult> QueryBy(string county, string state, string startDate, string endDate)
        {
            try
            {
                var startDt = ParseDateString(startDate);
                var endDt = ParseDateString(endDate);

                var dateRange = new DateRange(startDt, endDt);
                if (!string.IsNullOrEmpty(county))
                {
                    return Ok(await _dataRepository.QueryByCounty(county, dateRange));
                }
                else if (!string.IsNullOrEmpty(state))
                {
                    return Ok(await _dataRepository.QueryByState(state, dateRange));
                }
                else
                {
                    return NotFound();
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                return Problem();
            }
        }

        private DateTime? ParseDateString(string dateString)
        {
            if (string.IsNullOrEmpty(dateString)) return null;

            return DateTime.Parse(dateString);
        }
    }
}
