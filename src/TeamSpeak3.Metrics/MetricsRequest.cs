﻿using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Threading.Tasks;
using TeamSpeak3.Metrics.Query;

namespace TeamSpeak3.Metrics
{
    public static class MetricsRequest
    {
        public static Task Handle(HttpContext context)
        {
            var controller = context.RequestServices.GetService<ITeamSpeakMetrics>();

            var metrics = controller.Metrics;
            if (metrics == null || controller.IsError)
            {
                context.Response.StatusCode = 503;
                return context.Response.WriteAsync(JsonConvert.SerializeObject(new { Error = "No data available" }));
            }

            context.Response.StatusCode = 200;
            return context.Response.WriteAsync(JsonConvert.SerializeObject(metrics));
        }
    }
}
