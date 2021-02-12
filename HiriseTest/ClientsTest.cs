using Microsoft.VisualStudio.TestTools.UnitTesting;
using HiriseLib;

namespace HiriseTest
{
    [TestClass]
    [TestCategory("Local")]
    public class ClientsTest
    {
        [TestMethod]
        public void Login()
        {
            using(var connector = new Connector())
            {
                IClientSession session = connector.Login("User1", "Machine1");
                Assert.IsNotNull(session);
                Assert.IsTrue(session.IsLoggedIn);
                Assert.AreEqual("User1", session.Name);
                Assert.AreEqual("Machine1", session.Endpoint);

                IClient client = connector.GetClient("User1");
                Assert.IsNotNull(client);
                Assert.AreEqual("User1", client.Name);
                Assert.AreEqual("Machine1", client.LastLoginEndpoint);

                session.Dispose();
                Assert.IsFalse(session.IsLoggedIn);
            }
        }

        [TestMethod]
        public void GetUnknownClient()
        {
            using (var connector = new Connector())
            {
                IClient client = connector.GetClient("User2");
                Assert.IsNotNull(client);
                Assert.AreEqual("User2", client.Name);
                Assert.IsNull(client.LastLoginEndpoint);
                Assert.IsNull(client.LastLoginTimestamp);
            }
        }

        [TestMethod]
        public void GetKnownClient()
        {
            using (var connector = new Connector())
            {
                IClient client = connector.GetClient("User1");
                Assert.IsNotNull(client);
                Assert.AreEqual("User1", client.Name);
                Assert.IsNotNull(client.LastLoginEndpoint);
                Assert.IsNotNull(client.LastLoginTimestamp);
            }
        }
    }
}
