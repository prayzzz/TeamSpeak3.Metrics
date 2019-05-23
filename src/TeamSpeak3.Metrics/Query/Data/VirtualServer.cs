namespace TeamSpeak3.Metrics.Query.Data
{
    public class VirtualServer
    {
        public int VirtualServerClientsOnline { get; set; }

        public int VirtualServerId { get; set; }

        public int VirtualServerMaxclients { get; set; }

        public string VirtualServerName { get; set; }

        public int VirtualServerPort { get; set; }

        public string VirtualServerStatus { get; set; }

        public string VirtualServerUptime { get; set; }

        public bool VirtualServerAutoStart { get; set; }
        
        public bool VirtualServerMachineId { get; set; }
    }
}
