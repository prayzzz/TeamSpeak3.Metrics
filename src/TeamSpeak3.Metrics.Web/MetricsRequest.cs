using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace TeamSpeak3.Metrics.Web
{
    public static class MetricsRequest
    {
        public static Task Handle(HttpContext context)
        {
            // var controller = context.RequestServices.GetService<Teamspeak3Metrics>();
            //
            // var metrics = controller.Metrics;
            // if (metrics == null || controller.IsError)
            // {
            //     context.Response.StatusCode = 503;
            //     return context.Response.WriteAsync(JsonConvert.SerializeObject(new { Error = "No data available" }));
            // }
            //
            // context.Response.StatusCode = 200;
            return context.Response.WriteAsync(JsonConvert.SerializeObject(null));
        }
    }
}
