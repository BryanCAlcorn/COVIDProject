using COVIDData.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace COVIDData.Interfaces
{
    public interface ICovidDataSource
    {
        /// <summary>
        /// Get the COVID data
        /// </summary>
        /// <returns>List of COVID data</returns>
        Task<IList<CovidDataRow>> GetData();
    }
}
