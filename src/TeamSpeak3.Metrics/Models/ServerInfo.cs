namespace TeamSpeak3.Metrics.Models
{
    public class ServerInfo
    {
        public int ConnectionBandwidthReceivedLastMinuteTotal { get; set; }

        public int ConnectionBandwidthReceivedLastSecondTotal { get; set; }

        public int ConnectionBandwidthSentLastMinuteTotal { get; set; }

        public int ConnectionBandwidthSentLastSecondTotal { get; set; }

        public int ConnectionBytesReceivedControl { get; set; }

        public int ConnectionBytesReceivedKeepalive { get; set; }

        public int ConnectionBytesReceivedSpeech { get; set; }

        public int ConnectionBytesReceivedTotal { get; set; }

        public int ConnectionBytesSentControl { get; set; }

        public int ConnectionBytesSentKeepalive { get; set; }

        public int ConnectionBytesSentSpeech { get; set; }

        public int ConnectionBytesSentTotal { get; set; }

        public int ConnectionFiletransferBandwidthReceived { get; set; }

        public int ConnectionFiletransferBandwidthSent { get; set; }

        public int ConnectionFiletransferBytesReceivedTotal { get; set; }

        public int ConnectionFiletransferBytesSentTotal { get; set; }

        public int ConnectionPacketsReceivedControl { get; set; }

        public int ConnectionPacketsReceivedKeepalive { get; set; }

        public int ConnectionPacketsReceivedSpeech { get; set; }

        public int ConnectionPacketsReceivedTotal { get; set; }

        public int ConnectionPacketsSentControl { get; set; }

        public int ConnectionPacketsSentKeepalive { get; set; }

        public int ConnectionPacketsSentSpeech { get; set; }

        public int ConnectionPacketsSentTotal { get; set; }

        public int VirtualServerAntifloodPointsNeededCommandBlock { get; set; }

        public int VirtualServerAntifloodPointsNeededIpBlock { get; set; }

        public int VirtualServerAntifloodPointsNeededPluginBlock { get; set; }

        public int VirtualServerAntifloodPointsTickReduce { get; set; }

        public int VirtualServerAskForPrivilegekey { get; set; }

        public int VirtualServerAutostart { get; set; }

        public int VirtualServerChannelsOnline { get; set; }

        public int VirtualServerChannelTempDeleteDelayDefault { get; set; }

        public int VirtualServerClientConnections { get; set; }

        public int VirtualServerClientsOnline { get; set; }

        public int VirtualServerCodecEncryptionMode { get; set; }

        public int VirtualServerComplainAutobanCount { get; set; }

        public int VirtualServerComplainAutobanTime { get; set; }

        public int VirtualServerComplainRemoveTime { get; set; }

        public string VirtualServerCreated { get; set; }

        public int VirtualServerDefaultChannelAdminGroup { get; set; }

        public int VirtualServerDefaultChannelGroup { get; set; }

        public int VirtualServerDefaultServerGroup { get; set; }

        public ulong VirtualServerDownloadQuota { get; set; }

        public string VirtualServerFilebase { get; set; }

        public int VirtualServerFlagPassword { get; set; }

        public int VirtualServerHostbannerGfxInterval { get; set; }

        public string VirtualServerHostbannerGfxUrl { get; set; }

        public int VirtualServerHostbannerMode { get; set; }

        public string VirtualServerHostbannerUrl { get; set; }

        public string VirtualServerHostbuttonGfxUrl { get; set; }

        public string VirtualServerHostbuttonTooltip { get; set; }

        public string VirtualServerHostbuttonUrl { get; set; }

        public string VirtualServerHostmessage { get; set; }

        public int VirtualServerHostmessageMode { get; set; }

        public int VirtualServerIconId { get; set; }

        public int VirtualServerId { get; set; }

        public string VirtualServerIp { get; set; }

        public int VirtualServerLogChannel { get; set; }

        public int VirtualServerLogClient { get; set; }

        public int VirtualServerLogFiletransfer { get; set; }

        public int VirtualServerLogPermissions { get; set; }

        public int VirtualServerLogQuery { get; set; }

        public int VirtualServerLogServer { get; set; }

        public int VirtualServerMaxClients { get; set; }

        public ulong VirtualServerMaxDownloadTotalBandwidth { get; set; }

        public ulong VirtualServerMaxUploadTotalBandwidth { get; set; }

        public int VirtualServerMinClientsInChannelBeforeForcedSilence { get; set; }

        public int VirtualServerMinClientVersion { get; set; }

        public int VirtualServerMonthBytesDownloaded { get; set; }

        public int VirtualServerMonthBytesUploaded { get; set; }

        public string VirtualServerName { get; set; }

        public int VirtualServerNeededIdentitySecurityLevel { get; set; }

        public string VirtualServerPlattform { get; set; }

        public int VirtualServerPort { get; set; }

        public double VirtualServerPrioritySpeakerDimmModificator { get; set; }

        public int VirtualServerQueryClientConnections { get; set; }

        public int VirtualServerQueryClientsOnline { get; set; }

        public int VirtualServerReservedSlots { get; set; }

        public string VirtualServerStatus { get; set; }

        public int VirtualServerTotalBytesDownloaded { get; set; }

        public int VirtualServerTotalBytesUploaded { get; set; }

        public double VirtualServerTotalPacketlossControl { get; set; }

        public double VirtualServerTotalPacketlossKeepalive { get; set; }

        public double VirtualServerTotalPacketlossSpeech { get; set; }

        public double VirtualServerTotalPacketlossTotal { get; set; }

        public double VirtualServerTotalPing { get; set; }

        public string VirtualServerUniqueIdentifier { get; set; }

        public ulong VirtualServerUploadQuota { get; set; }

        public int VirtualServerUptime { get; set; }

        public string VirtualServerVersion { get; set; }

        public int VirtualServerWeblistEnabled { get; set; }

        public string VirtualServerWelcomemessage { get; set; }
    }
}
