using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TeamSpeak3.Metrics.Test.Util;

namespace TeamSpeak3.Metrics.Test
{
    [TestClass]
    public class QueryConnectionTest
    {
        [TestMethod]
        public async Task SendAndReceive()
        {
            var telnetServer = new TelnetServer();
            telnetServer.Setup("command", "response");

            var loggerFactory = TH.MockLoggerFactory();

            var queryConnectionFactory = new QueryConnectionFactory(loggerFactory.Object);

            using (var connection = await queryConnectionFactory.Create(telnetServer.Ip, telnetServer.Port))
            {
                // Act
                var response = await connection.SendAndReceive("command");

                // Assert
                Assert.AreEqual("response", response);
            }

            telnetServer.Dispose();
        }
    }
}
