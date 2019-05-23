using Moq;

namespace TeamSpeak3.Metrics.Test
{
    // ReSharper disable once InconsistentNaming
    public static class TH
    {
        public static Mock<T> CreateMock<T>() where T : class
        {
            return new Mock<T>(MockBehavior.Strict);
        }
    }
}
