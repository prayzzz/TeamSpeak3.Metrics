using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TeamSpeak3.Metrics.Connection;

namespace TeamSpeak3.Metrics.Test.Connection
{
    [TestClass]
    public class TeamspeakConnectionTest
    {
        [TestMethod]
        public void TestConnect()
        {
            using (new TelnetServer())
            using (var connection = new TeamspeakConnection())
            {
                var isConnected = connection.Connect(TelnetServer.Ip, TelnetServer.Port);

                Assert.IsTrue(isConnected);
            }
        }

        [TestMethod]
        public async Task TestLogin()
        {
            using (var server = new TelnetServer())
            using (var connection = new TeamspeakConnection())
            {
                if (!connection.Connect(TelnetServer.Ip, TelnetServer.Port))
                {
                    Assert.Fail("Missing connection");
                }

                server.Setup("login user pw", "error id=0 msg=ok");

                var result = await connection.Login("user", "pw");

                Assert.AreEqual(0, result.ErrorId);
                Assert.AreEqual("ok", result.ErrorMessage);
            }
        }

        [TestMethod]
        public async Task TestUse()
        {
            using (var server = new TelnetServer())
            using (var connection = new TeamspeakConnection())
            {
                if (!connection.Connect(TelnetServer.Ip, TelnetServer.Port))
                {
                    Assert.Fail("Missing connection");
                }

                server.Setup("use 1", "error id=0 msg=ok");

                var result = await connection.Use(1);

                Assert.AreEqual(0, result.ErrorId);
                Assert.AreEqual("ok", result.ErrorMessage);
            }
        }
    }
}