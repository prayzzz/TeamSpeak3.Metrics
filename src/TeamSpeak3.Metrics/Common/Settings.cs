namespace TeamSpeak3.Metrics.Common
{
    public class AppConfiguration
    {
        public LoggingConfiguration Logging { get; set; }

        public TeamSpeakConfiguration TeamSpeak { get; set; }
    }

    public class LoggingConfiguration
    {
        public string PathFormat { get; set; }
    }

    public class TeamSpeakConfiguration
    {
        public string Host { get; set; }

        public int Port { get; set; }

        public string QueryPassword { get; set; }

        public int QueryPort { get; set; }

        public string QueryUsername { get; set; }
    }
}