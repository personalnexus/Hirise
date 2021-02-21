using HiriseLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;

namespace HiriseTest
{
    [TestClass]
    public class TreeUpdatorTest
    {
        [TestMethod]
        public void PublishAndSubscribe()
        {
            using (Connector connector1 = TreeTest.InitializeConnector(out IClientSession session1))
            using (Connector connector2 = TreeTest.InitializeConnector(out IClientSession session2))
            {
                IItem itemOnConnector1 = connector1.Tree.GetOrAddItem("TreeUpdatorTest.Item1");
                Assert.IsTrue(itemOnConnector1.AddSubscriberAsync(session1).Result);

                IItem itemOnConnector2 = connector2.Tree.GetOrAddItem("TreeUpdatorTest.Item1");
                itemOnConnector2.DataAsString = "TestData";
                itemOnConnector2.StoreAsync(session2);
                itemOnConnector2.DataAsString = "TestData2";

                // Update reaches connector1
                Thread.Sleep(333);
                Assert.AreEqual("TestData", itemOnConnector1.DataAsString);

                // But not connector2 which had already a newer one
                Assert.AreEqual("TestData2", itemOnConnector2.DataAsString);
            }
        }
    }
}
