using Microsoft.VisualStudio.TestTools.UnitTesting;
using prayzzz.Common.Unit;
using System.Collections.Generic;
using System.Threading.Tasks;
using TeamSpeak3.Metrics.Query;

namespace TeamSpeak3.Metrics.Test.Query
{
    [TestClass]
    public class TeamspeakConnectionTest
    {
        [TestMethod]
        public async Task TestClientList()
        {
            using (var server = new TelnetServer())
            using (var connection = new TeamSpeakConnection(new ConsoleLogger<TeamSpeakConnection>()))
            {
                var isConnected = await connection.Connect(server.Ip, server.Port);
                if (!isConnected)
                {
                    Assert.Fail("Missing connection");
                }

                var user1 = new Dictionary<string, string>
                {
                    { "clid", "3" },
                    { "cid", "4" },
                    { "client_nickname", "prayzzz\\s\\p\\sPatrick" },
                    { "client_type", "0" }
                };

                var user2 = new Dictionary<string, string>
                {
                    { "clid", "1" },
                    { "cid", "5" },
                    { "client_nickname", "RussianBeeAdmin" },
                    { "client_type", "1" }
                };

                server.Setup("clientlist",
                             Translator.Translate(new List<Dictionary<string, string>> { user1, user2 }) +
                             Response.NewLine +
                             "error id=0 msg=ok");

                var result = await connection.ClientList();

                Assert.AreEqual(0, result.ErrorId);
                Assert.AreEqual("ok", result.ErrorMessage);
                Assert.AreEqual(2, result.Data.Count);

                var client1 = result.Data[0];
                Assert.AreEqual(4, client1.Cid);
                Assert.AreEqual(3, client1.Clid);
                Assert.AreEqual("prayzzz | Patrick", client1.ClientNickname);
                Assert.AreEqual(0, client1.ClientType);

                var client2 = result.Data[1];
                Assert.AreEqual(5, client2.Cid);
                Assert.AreEqual(1, client2.Clid);
                Assert.AreEqual("RussianBeeAdmin", client2.ClientNickname);
                Assert.AreEqual(1, client2.ClientType);
            }
        }

        [TestMethod]
        public async Task TestConnect()
        {
            using (var server = new TelnetServer())
            using (var connection = new TeamSpeakConnection(new ConsoleLogger<TeamSpeakConnection>()))
            {
                var isConnected = await connection.Connect(server.Ip, server.Port);

                Assert.IsTrue(isConnected);
            }
        }

        [TestMethod]
        public async Task TestLogin()
        {
            using (var server = new TelnetServer())
            using (var connection = new TeamSpeakConnection(new ConsoleLogger<TeamSpeakConnection>()))
            {
                var isConnected = await connection.Connect(server.Ip, server.Port);
                if (!isConnected)
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
            using (var connection = new TeamSpeakConnection(new ConsoleLogger<TeamSpeakConnection>()))
            {
                var isConnected = await connection.Connect(server.Ip, server.Port);
                if (!isConnected)
                {
                    Assert.Fail("Missing connection");
                }

                server.Setup("use port=9987", "error id=0 msg=ok");
                var result = await connection.Use(9987);

                Assert.AreEqual(0, result.ErrorId);
                Assert.AreEqual("ok", result.ErrorMessage);
            }
        }
    }
}
