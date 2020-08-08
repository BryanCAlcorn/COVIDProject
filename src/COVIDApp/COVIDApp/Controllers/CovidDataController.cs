using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using COVIDData;
using Microsoft.AspNetCore.Http;
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
        public void QueryBy(string county, string state, DateTime startDate, DateTime endDate)
        {

        }
    }
}
