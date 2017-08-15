namespace TeamSpeak3.Metrics.Common
{
    public class AppSettings
    {
        public TeamSpeakSettings TeamSpeak { get; set; }
    }

    public class TeamSpeakSettings
    {
        public string Ip { get; set; }

        public int Port { get; set; }

        public string QueryPassword { get; set; }

        public int QueryPort { get; set; }

        public string QueryUsername { get; set; }
    }
}