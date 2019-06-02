namespace TeamSpeak3.Metrics.Mapping
{
    public class DataResponse<T> : QueryResponse where T : class
    {
        internal DataResponse(QueryResponse status) : base(status.Id, status.Message)
        {
            Data = null;
        }

        internal DataResponse(QueryResponse status, T data) : base(status.Id, status.Message)
        {
            Data = data;
        }

        public T Data { get; }

        public bool HasData => Data != null;
    }

    public class StatusResponse : QueryResponse
    {
        internal StatusResponse(int id, string message) : base(id, message)
        {
        }
    }

    public abstract class QueryResponse
    {
        protected QueryResponse(int id, string message)
        {
            Id = id;
            Message = message;
        }

        public int Id { get; }

        public bool IsSuccess => Id == 0;

        public string Message { get; }
    }
}
