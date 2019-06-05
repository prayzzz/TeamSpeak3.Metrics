using System.Collections.Generic;
using TeamSpeak3.Metrics.Models;

namespace TeamSpeak3.Metrics.AspNetCore.Hosted
{
    public interface IMetricCollectorCache
    {
        IEnumerable<TeamSpeak3Metrics> Current { get; }
    }
}
