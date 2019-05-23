using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TeamSpeak3.Metrics.Configuration;
using TeamSpeak3.Metrics.v2;

namespace TeamSpeak3.Metrics.Test.v2
{
    [TestClass]
    public class GatewayTest
    {
        private static readonly ServerConfiguration ServerConfiguration = new ServerConfiguration
        {
            Host = "localhost",
            QueryPort = 1234,
            QueryUsername = "query-user",
            QueryPassword = "query-password"
        };

        [TestMethod]
        public async Task GetClientList()
        {
            const int vServerPort = 4242;

            var logger = new ConsoleLogger<Gateway>();
            var connection = TH.CreateMock<IQueryConnection>();
            var factory = TH.CreateMock<IQueryConnectionFactory>();
            factory.Setup(x => x.Create(ServerConfiguration.Host, ServerConfiguration.QueryPort)).ReturnsAsync(connection.Object);

            connection.Setup(x => x.SendAndReceive($"login {ServerConfiguration.QueryUsername} {ServerConfiguration.QueryPassword}"))
                      .ReturnsAsync("error id=0 msg=ok");
            connection.Setup(x => x.SendAndReceive($"use port={vServerPort}"))
                      .ReturnsAsync("error id=0 msg=ok");
            connection.Setup(x => x.SendAndReceive("clientlist"))
                      .ReturnsAsync("clid=3 cid=4 client_nickname=prayzzz\\s\\p\\sPatrick client_type=0|clid=1 cid=5 client_nickname=Admin client_type=1");
            connection.Setup(x => x.Dispose());

            // Act
            var gateway = new Gateway(factory.Object, logger, ServerConfiguration);
            var result = await gateway.GetClientList(vServerPort);

            // Assert
            Assert.IsTrue(result.Any(c => c.ClientNickname == "Admin"));
            Assert.IsTrue(result.Any(c => c.ClientNickname == "prayzzz | Patrick"));
        }

        [TestMethod]
        public async Task GetClientListLoginInvalid()
        {
            const int vServerPort = 4242;

            var logger = new ConsoleLogger<Gateway>();
            var connection = TH.CreateMock<IQueryConnection>();
            var factory = TH.CreateMock<IQueryConnectionFactory>();
            factory.Setup(x => x.Create(ServerConfiguration.Host, ServerConfiguration.QueryPort)).ReturnsAsync(connection.Object);

            connection.Setup(x => x.SendAndReceive($"login {ServerConfiguration.QueryUsername} {ServerConfiguration.QueryPassword}"))
                      .ReturnsAsync("error id=520 msg=invalid\\sloginname\\sor\\spassword");
            connection.Setup(x => x.Dispose());

            // Act
            var gateway = new Gateway(factory.Object, logger, ServerConfiguration);
            var result = await Assert.ThrowsExceptionAsync<Exception>(() => gateway.GetClientList(vServerPort));

            // Assert
            Assert.AreEqual("Couldn't login with query credentials 'invalid loginname or password'", result.Message);
        }
    }
}
