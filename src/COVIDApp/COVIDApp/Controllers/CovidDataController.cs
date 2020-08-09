using System;
using System.Threading.Tasks;
using COVIDData.Exceptions;
using COVIDData.Interfaces;
using COVIDData.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace COVIDApp.Controllers
{
    /// <summary>
    /// Controller for COVID Data queries
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CovidDataController : ControllerBase
    {
        private readonly ICovidDataRepository _dataRepository;
        private readonly ILogger<CovidDataController> _logger;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="covidDataRepository">Data repository</param>
        /// <param name="logger">Logger</param>
        public CovidDataController(ILogger<CovidDataController> logger, ICovidDataRepository covidDataRepository)
        {
            _logger = logger;
            _dataRepository = covidDataRepository;
        }

        /// <summary>
        /// Query the Aggregate COVID Data by county OR state and a given date range.
        /// </summary>
        /// <param name="county">County to query, either provide this OR state</param>
        /// <param name="state">State to query, either provide this OR county</param>
        /// <param name="startDate">Start date for the range to query. <br/>
        /// If left out or date is set to before earliest available date, <br/>
        /// will use the minimum available date in the data.</param>
        /// <param name="endDate">End date for the range to query. <br/>
        /// If left out or date is set to after latest available date, <br/>
        /// will use the maximum available date in the data.</param>
        /// <returns>Aggregated COVID data for the County or State within the given date range</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK),
            ProducesResponseType(StatusCodes.Status400BadRequest),
            ProducesResponseType(StatusCodes.Status404NotFound),
            ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
                return Problem(statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Query the Daily Breakdown COVID Data by county OR state and a given date range.
        /// </summary>
        /// <param name="county">County to query, either provide this OR state</param>
        /// <param name="state">State to query, either provide this OR county</param>
        /// <param name="startDate">Start date for the range to query. <br/>
        /// If left out or date is set to before earliest available date, <br/>
        /// will use the minimum available date in the data.</param>
        /// <param name="endDate">End date for the range to query. <br/>
        /// If left out or date is set to after latest available date, <br/>
        /// will use the maximum available date in the data.</param>
        /// <returns>Daily Breakdown COVID data for the County or State within the given date range</returns>
        [HttpGet, Route("daily")]
        [ProducesResponseType(StatusCodes.Status200OK),
            ProducesResponseType(StatusCodes.Status400BadRequest),
            ProducesResponseType(StatusCodes.Status404NotFound),
            ProducesResponseType(StatusCodes.Status500InternalServerError)]
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

        /// <summary>
        /// Query the Daily Rate of Change COVID Data by county OR state and a given date range.
        /// </summary>
        /// <param name="county">County to query, either provide this OR state</param>
        /// <param name="state">State to query, either provide this OR county</param>
        /// <param name="startDate">Start date for the range to query. <br/>
        /// If left out or date is set to before earliest available date, <br/>
        /// will use the minimum available date in the data.</param>
        /// <param name="endDate">End date for the range to query. <br/>
        /// If left out or date is set to after latest available date, <br/>
        /// will use the maximum available date in the data.</param>
        /// <returns>Daily Rate of Change COVID data for the County or State within the given date range</returns>
        [HttpGet, Route("change")]
        [ProducesResponseType(StatusCodes.Status200OK),
            ProducesResponseType(StatusCodes.Status400BadRequest),
            ProducesResponseType(StatusCodes.Status404NotFound),
            ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
