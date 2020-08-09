using System.Threading.Tasks;

namespace COVIDData
{
    public interface ICovidDataRepository
    {
        Task<CovidQueryResult> QueryByCounty(string county, DateRange range);

        Task<CovidQueryResult> QueryByState(string state, DateRange range);

        Task<DailyBreakdownResult> GetDailyBreakdownByCounty(string county, DateRange range);

        Task<DailyBreakdownResult> GetDailyBreakdownByState(string state, DateRange range);

        Task<RateOfChangeResult> GetRateOfChangeByCounty(string county, DateRange range);

        Task<RateOfChangeResult> GetRateOfChangeByState(string state, DateRange range);
    }
}
