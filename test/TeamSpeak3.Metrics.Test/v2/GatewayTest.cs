using System;
using System.Linq;
using System.Reflection;
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
                      .ReturnsAsync(TH.ReadEmbeddedFile(Assembly.GetExecutingAssembly(), "Resources.GatewayTest.GetClientList.txt"));
            connection.Setup(x => x.Dispose());

            // Act
            var gateway = new Gateway(factory.Object, logger, ServerConfiguration);
            var result = await gateway.GetClientList(vServerPort);

            // Assert
            Assert.IsTrue(result.Any(c => c.ClientNickname == "Admin"));
            Assert.IsTrue(result.Any(c => c.Clid == 1));
            Assert.IsTrue(result.Any(c => c.ClientNickname == "prayzzz | Patrick"));
            Assert.IsTrue(result.Any(c => c.Clid == 3));
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

        [TestMethod]
        public async Task GetServerInfo()
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
            connection.Setup(x => x.SendAndReceive("serverinfo"))
                      .ReturnsAsync(TH.ReadEmbeddedFile(Assembly.GetExecutingAssembly(), "Resources.GatewayTest.GetServerInfo.txt"));
            connection.Setup(x => x.Dispose());

            // Act
            var gateway = new Gateway(factory.Object, logger, ServerConfiguration);
            var result = await gateway.GetServerInfo(vServerPort);

            // Assert
            Assert.AreEqual("TeamSpeak ]I[ Server", result.VirtualServerName);
            Assert.AreEqual(385, result.VirtualServerUptime);
            Assert.AreEqual(0.0, result.VirtualServerTotalPing);
        }

        [TestMethod]
        public async Task GetServerList()
        {
            const int vServerPort = 4242;

            var logger = new ConsoleLogger<Gateway>();
            var connection = TH.CreateMock<IQueryConnection>();
            var factory = TH.CreateMock<IQueryConnectionFactory>();
            factory.Setup(x => x.Create(ServerConfiguration.Host, ServerConfiguration.QueryPort)).ReturnsAsync(connection.Object);

            connection.Setup(x => x.SendAndReceive($"login {ServerConfiguration.QueryUsername} {ServerConfiguration.QueryPassword}"))
                      .ReturnsAsync("error id=0 msg=ok");
            connection.Setup(x => x.SendAndReceive("serverlist"))
                      .ReturnsAsync(TH.ReadEmbeddedFile(Assembly.GetExecutingAssembly(), "Resources.GatewayTest.GetServerList.txt"));
            connection.Setup(x => x.Dispose());

            // Act
            var gateway = new Gateway(factory.Object, logger, ServerConfiguration);
            var result = await gateway.GetServerList();

            // Assert
            Assert.IsTrue(result.Any(c => c.VirtualServerName == "TeamSpeak ]I[ Server"));
            Assert.IsTrue(result.Any(c => c.VirtualServerPort == 9987));
            Assert.IsTrue(result.Any(c => c.VirtualServerName == "TeamSpeak ]I[ Server2"));
            Assert.IsTrue(result.Any(c => c.VirtualServerPort == 9988));
        }
    }
}
