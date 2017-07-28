using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
            using (var connection = new TeamSpeakQuery())
            {
                if (!connection.Connect(server.Ip, server.Port))
                {
                    Assert.Fail("Missing connection");
                }

                server.Setup("clientlist",
                             "clid=3 cid=4 client_database_id=26 client_nickname=prayzzz\\s\\p\\sPatrick client_type=0|clid=5 cid=1 client_database_id=25 client_nickname=RussianBeeAdmin client_type=1" +
                             Environment.NewLine +
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
                Assert.AreEqual(1, client2.Cid);
                Assert.AreEqual(5, client2.Clid);
                Assert.AreEqual("RussianBeeAdmin", client2.ClientNickname);
                Assert.AreEqual(1, client2.ClientType);
            }
        }

        [TestMethod]
        public void TestConnect()
        {
            using (var server = new TelnetServer())
            using (var connection = new TeamSpeakQuery())
            {
                var isConnected = connection.Connect(server.Ip, server.Port);

                Assert.IsTrue(isConnected);
            }
        }

        [TestMethod]
        public async Task TestLogin()
        {
            using (var server = new TelnetServer())
            using (var connection = new TeamSpeakQuery())
            {
                if (!connection.Connect(server.Ip, server.Port))
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
            using (var connection = new TeamSpeakQuery())
            {
                if (!connection.Connect(server.Ip, server.Port))
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