using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TeamSpeak3.Metrics.Models;

namespace TeamSpeak3.Metrics.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MetricsController : ControllerBase
    {
        private readonly IMetricCollector _metricCollector;

        public MetricsController(IMetricCollector metricCollector)
        {
            _metricCollector = metricCollector;
        }

        [HttpGet]
        public async Task<IEnumerable<TeamSpeak3Metrics>> Get()
        {
            return await _metricCollector.Collect();
        }
    }
}
