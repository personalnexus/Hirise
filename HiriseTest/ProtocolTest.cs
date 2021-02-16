using Microsoft.VisualStudio.TestTools.UnitTesting;
using HiriseLib.Tree;
using HiriseLib;

namespace HiriseTest
{
    [TestClass]
    public class ProtocolTest
    {
        [TestMethod]
        public void GetFolderPath()
        {
            var folder1 = new Folder("Folder1", null, null);
            var folder2 = new Folder("Folder2", folder1, null);
            var folder3 = new Folder("Folder3", folder2, null);
            Assert.AreEqual("Folder1", folder1.Path);
            Assert.AreEqual("Folder1:Folder2", folder2.Path);
            Assert.AreEqual("Folder1:Folder2:Folder3", folder3.Path);
        }

        [TestMethod]
        public void GetItemPath()
        {
            var folder1 = new Folder("Folder1", null, null);
            var folder2 = new Folder("Folder2", folder1, null);
            var folder3 = new Folder("Folder3", folder2, null);
            var item = new Item("Item", folder3, null);
            Assert.AreEqual("Folder1:Folder2:Folder3.Item", item.Path);
        }

        [TestMethod]
        public void ElementsToStringList()
        {
            var folder1 = new Folder("Folder1", null, null);
            var folder2 = new Folder("Folder2", folder1, null);
            var folder3 = new Folder("Folder3", folder2, null);
            Assert.AreEqual("Folder1:Folder2:Folder3\nFolder1", Protocol.ElementsToStringList(new[] { folder3, folder1 }));
        }

        [TestMethod]
        public void SplitFolderAndItems()
        {
            Protocol.SplitFolderAndItems("Node1:Node2:Node3.item1", out string[] folders, out string itemName);
            CollectionAssert.AreEqual(new[] { "Node1", "Node2", "Node3" }, folders);
            Assert.AreEqual("item1", itemName);
        }
    }
}
