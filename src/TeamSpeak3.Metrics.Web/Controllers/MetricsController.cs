using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using TeamSpeak3.Metrics.AspNetCore.Hosted;
using TeamSpeak3.Metrics.Models;

namespace TeamSpeak3.Metrics.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MetricsController : ControllerBase
    {
        private readonly IMetricCollectorCache _metricCollectorCache;

        public MetricsController(IMetricCollectorCache metricCollectorCache)
        {
            _metricCollectorCache = metricCollectorCache;
        }

        [HttpGet]
        public IEnumerable<TeamSpeak3Metrics> Get()
        {
            return _metricCollectorCache.Current;
        }
    }
}
