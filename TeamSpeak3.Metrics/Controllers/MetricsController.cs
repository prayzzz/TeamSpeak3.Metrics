using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace TeamSpeak3.Metrics.Controllers
{
    [Route("api/[controller]")]
    public class MetricsController : Controller
    {
        private readonly PeriodicDataCollector _collector;

        public MetricsController(PeriodicDataCollector collector)
        {
            _collector = collector;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var metrics = new VirtualServerMetrics();
            metrics.ClientsOnline = _collector.VirtualServer.VirtualServerClientsOnline;
            metrics.Clients = _collector.Clients.Select(x => x.ClientNickname);
            metrics.ServerName = _collector.VirtualServer.VirtualServerName;
            metrics.BytesSent = _collector.VirtualServer.ConnectionBytesSentTotal;
            metrics.BytesReceived = _collector.VirtualServer.ConnectionBytesReceivedTotal;
            
            return Json(metrics);
        }
    }
}
