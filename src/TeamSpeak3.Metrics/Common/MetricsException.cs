using System;

namespace TeamSpeak3.Metrics.Common
{
    public class MetricsException : Exception
    {
        internal MetricsException(string message) : base(message)
        {
        }

        internal MetricsException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
