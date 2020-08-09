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

                if (!string.IsNullOrEmpty(county) && string.IsNullOrEmpty(state))
                {
                    return Ok(await _dataRepository.QueryByCounty(county, dateRange));
                }
                else if (!string.IsNullOrEmpty(state))
                {
                    return Ok(await _dataRepository.QueryByState(state, dateRange));
                }
                else
                {
                    return BadRequest($"Must use either {nameof(county)} or {nameof(state)}");
                }
            }
            catch (DatesOutOfRangeException)
            {
                return BadRequest($"{nameof(startDate)} must be earlier than {nameof(endDate)}");
            }
            catch (DataNotFoundException dex)
            {
                Console.WriteLine(dex);
                return NotFound(string.IsNullOrEmpty(county) ? state : county);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                return Problem();
            }
        }

        [HttpGet, Route("daily")]
        public async Task<IActionResult> DailyBreakdown(string county, string state, DateTime? startDate, DateTime? endDate)
        {
            try
            {
                var dateRange = new DateRange(startDate, endDate);

                if (!string.IsNullOrEmpty(county) && string.IsNullOrEmpty(state))
                {
                    return Ok(await _dataRepository.GetDailyBreakdownByCounty(county, dateRange));
                }
                else if (!string.IsNullOrEmpty(state))
                {
                    return Ok(await _dataRepository.GetDailyBreakdownByState(state, dateRange));
                }
                else
                {
                    return BadRequest($"Must use either {nameof(county)} or {nameof(state)}");
                }
            }
            catch (DatesOutOfRangeException)
            {
                return BadRequest($"{nameof(startDate)} must be earlier than {nameof(endDate)}");
            }
            catch (DataNotFoundException dex)
            {
                Console.WriteLine(dex);
                return NotFound(string.IsNullOrEmpty(county) ? state : county);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return Problem();
            }
        }

        [HttpGet, Route("change")]
        public async Task<IActionResult> RateOfChange(string county, string state, DateTime? startDate, DateTime? endDate)
        {
            try
            {
                var dateRange = new DateRange(startDate, endDate);

                if (!string.IsNullOrEmpty(county) && string.IsNullOrEmpty(state))
                {
                    return Ok(await _dataRepository.GetRateOfChangeByCounty(county, dateRange));
                }
                else if (!string.IsNullOrEmpty(state))
                {
                    return Ok(await _dataRepository.GetRateOfChangeByState(state, dateRange));
                }
                else
                {
                    return BadRequest($"Must use either {nameof(county)} or {nameof(state)}");
                }
            }
            catch (DatesOutOfRangeException)
            {
                return BadRequest($"{nameof(startDate)} must be earlier than {nameof(endDate)}");
            }
            catch (DataNotFoundException dex)
            {
                Console.WriteLine(dex);
                return NotFound(string.IsNullOrEmpty(county) ? state : county);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return Problem();
            }
        }
    }
}
