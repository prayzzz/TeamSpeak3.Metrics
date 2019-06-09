using System.IO;
using System.Reflection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;

namespace TeamSpeak3.Metrics.Test.Util
{
    // ReSharper disable once InconsistentNaming
    public static class TH
    {
        public static Mock<T> CreateMock<T>() where T : class
        {
            return new Mock<T>(MockBehavior.Strict);
        }

        public static Mock<ILoggerFactory> MockLoggerFactory()
        {
            var loggerFactory = CreateMock<ILoggerFactory>();
            loggerFactory.Setup(x => x.CreateLogger(It.IsAny<string>())).Returns(NullLogger.Instance);

            return loggerFactory;
        }

        public static Mock<IOptionsMonitor<T>> MockOptionsMonitor<T>(T options)
        {
            var optionsMonitor = CreateMock<IOptionsMonitor<T>>();
            optionsMonitor.SetupGet(x => x.CurrentValue).Returns(options);

            return optionsMonitor;
        }

        public static string ReadEmbeddedFile(Assembly assembly, string filePath)
        {
            var name = assembly.GetName().Name;
            using (var stream = assembly.GetManifestResourceStream(name + "." + filePath))
            using (var str = new StreamReader(stream))
            {
                return str.ReadToEnd();
            }
        }
    }
}
