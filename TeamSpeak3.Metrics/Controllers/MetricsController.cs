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
            var metrics = new VirtualServerMetrics
            {
                BytesSent = _collector.VirtualServer.ConnectionBytesSentTotal,
                BytesReceived = _collector.VirtualServer.ConnectionBytesReceivedTotal,
                ClientsOnline = _collector.VirtualServer.VirtualServerClientsOnline,
                Clients = _collector.Clients.Select(x => x.ClientNickname),
                CollectedAt = _collector.CollectedAt,
                CollectionDuration = _collector.CollectionDuration,
                ServerId = _collector.VirtualServer.VirtualServerId,
                ServerName = _collector.VirtualServer.VirtualServerName,
                Status = _collector.VirtualServer.VirtualServerStatus,
                TotalPing = _collector.VirtualServer.VirtualServerTotalPing,
                Uptime = _collector.VirtualServer.VirtualServerUptime
            };

            return Json(metrics);
        }
    }
}