namespace TeamSpeak3.Metrics.v2
{
    public class Client
    {
        public Client(string client_nickname)
        {
            ClientNickname = client_nickname;
        }

        public string ClientNickname { get; }
    }
}
