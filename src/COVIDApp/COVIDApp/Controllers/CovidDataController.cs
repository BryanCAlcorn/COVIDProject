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
        public async Task<IActionResult> QueryBy(string county, string state, DateTime? startDate, DateTime? endDate)
        {
            try
            {
                var dateRange = new DateRange(startDate, endDate);
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
    }
}
