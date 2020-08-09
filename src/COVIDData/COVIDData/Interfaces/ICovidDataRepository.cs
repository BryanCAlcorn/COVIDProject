using COVIDData.Models;
using System.Threading.Tasks;

namespace COVIDData.Interfaces
{
    public interface ICovidDataRepository
    {
        /// <summary>
        /// Query the COVID Data by county for a given date range.
        /// </summary>
        /// <param name="county">County to query</param>
        /// <param name="range">Date range to query</param>
        /// <returns>Aggregate COVID data for the county &amp; date range</returns>
        Task<CovidQueryResult> QueryByCounty(string county, DateRange range);

        /// <summary>
        /// Query the COVID Data by state for a given date range.
        /// </summary>
        /// <param name="state">State to query</param>
        /// <param name="range">Date range to query</param>
        /// <returns>Aggregate COVID data for the state &amp; date range</returns>
        Task<CovidQueryResult> QueryByState(string state, DateRange range);

        /// <summary>
        /// Gets a daily breakdown of the COVID Data by county for a given date range.
        /// </summary>
        /// <param name="county">County to query</param>
        /// <param name="range">Date range to query</param>
        /// <returns>Number of cases per day and total change per day for the county &amp; date range</returns>
        Task<DailyBreakdownResult> GetDailyBreakdownByCounty(string county, DateRange range);

        /// <summary>
        /// Gets a daily breakdown of the COVID Data by state for a given date range.
        /// </summary>
        /// <param name="state">State to query</param>
        /// <param name="range">Date range to query</param>
        /// <returns>Number of cases per day and total change per day for the state &amp; date range</returns>
        Task<DailyBreakdownResult> GetDailyBreakdownByState(string state, DateRange range);

        /// <summary>
        /// Gets the rate of change of the COVID date over time by county for a given date range.
        /// </summary>
        /// <param name="county">County to query</param>
        /// <param name="range">Date range to query</param>
        /// <returns>Rate of change (value and percentage) per day for the county &amp; date range</returns>
        Task<RateOfChangeResult> GetRateOfChangeByCounty(string county, DateRange range);

        /// <summary>
        /// Gets the rate of change of the COVID date over time by state for a given date range.
        /// </summary>
        /// <param name="state">State to query</param>
        /// <param name="range">Date range to query</param>
        /// <returns>Rate of change (value and percentage) per day for the state &amp; date range</returns>
        Task<RateOfChangeResult> GetRateOfChangeByState(string state, DateRange range);
    }
}
