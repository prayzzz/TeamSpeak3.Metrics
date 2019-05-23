using System;

namespace TeamSpeak3.Metrics.v2
{
    public class TeamSpeak3MetricsException : Exception
    {
        public TeamSpeak3MetricsException(string message) : base(message)
        {
        }
        
        public TeamSpeak3MetricsException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
