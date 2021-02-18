using HiriseLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace HiriseTest
{
    [TestClass]
    public class ItemDataTest
    {
        [TestMethod]
        public async Task DataAsItemList()
        {
            using (var connector = new Connector())
            {
                IClientSession clientSession = connector.Login("User1", "");

                IItem item1 = connector.Tree.GetOrAddItem("Node1\\Node2\\Node3.item1");
                Assert.AreEqual("Node1\\Node2\\Node3.item1", item1.Path);
                IItem item2 = connector.Tree.GetOrAddItem("Node2\\Node4.item2");
                IItem item3 = connector.Tree.GetOrAddItem("Node1\\Node3.item3");

                item1.DataAsItemList = new[] { item2, item3 };
                await item1.StoreAsync(clientSession);
                await item1.LoadAsync();
                Assert.AreEqual("Node2\\Node4.item2\nNode1\\Node3.item3", item1.DataAsString);
            }
            return;
        }
    }
}
