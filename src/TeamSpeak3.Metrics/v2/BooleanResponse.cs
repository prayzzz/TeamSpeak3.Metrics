namespace TeamSpeak3.Metrics.v2
{
    public class BooleanResponse
    {
        public BooleanResponse(int id, string message)
        {
            Id = id;
            Message = Replacer.Replace(message);
        }

        public int Id { get; }

        public bool IsSuccess => Id == 0;

        public string Message { get; }
    }
}
