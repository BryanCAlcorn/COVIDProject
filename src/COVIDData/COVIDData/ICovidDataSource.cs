using System.Collections.Generic;
using System.Threading.Tasks;

namespace COVIDData
{
    public interface ICovidDataSource
    {
        Task<IList<CovidDataRow>> GetData();
    }
}
