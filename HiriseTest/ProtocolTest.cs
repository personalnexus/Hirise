using Microsoft.VisualStudio.TestTools.UnitTesting;
using HiriseLib.Tree;
using HiriseLib;
using System;
using System.Linq;

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
            Assert.AreEqual("Folder1\\Folder2", folder2.Path);
            Assert.AreEqual("Folder1\\Folder2\\Folder3", folder3.Path);
        }

        [TestMethod]
        public void GetItemPath()
        {
            var folder1 = new Folder("Folder1", null, null);
            var folder2 = new Folder("Folder2", folder1, null);
            var folder3 = new Folder("Folder3", folder2, null);
            var item = new Item("Item", folder3, null);
            Assert.AreEqual("Folder1\\Folder2\\Folder3.Item", item.Path);
        }

        [TestMethod]
        public void ElementsToStringList()
        {
            var folder1 = new Folder("Folder1", null, null);
            var folder2 = new Folder("Folder2", folder1, null);
            var folder3 = new Folder("Folder3", folder2, null);
            Assert.AreEqual("Folder1\\Folder2\\Folder3\nFolder1", Protocol.ElementsToStringList(new[] { folder3, folder1 }));
        }

        [TestMethod]
        public void SplitFolderAndItems()
        {
            Protocol.SplitFolderAndItems("Node1\\Node2\\Node3.item1", out string[] folders, out string itemName);
            CollectionAssert.AreEqual(new[] { "Node1", "Node2", "Node3" }, folders);
            Assert.AreEqual("item1", itemName);
        }

        [TestMethod]
        public void DateToStringToDate()
        {
            DateTime now = DateTime.Now;
            string nowAsString = DateAsString(now);
            DateTime actual = StringAsDate(nowAsString);
            Assert.AreEqual(now.ToString("dd.MM.yyyy HH:mm:ss.fff"), actual.ToString("dd.MM.yyyy HH:mm:ss.fff"));
        }

        private string DateAsString(DateTime input)
        {
            double oa = input.ToOADate();
            byte[] bytes = BitConverter.GetBytes(oa);
            char[] chars = bytes.Select(b => (char)b).ToArray();
            return new string(chars);
        }

        private DateTime StringAsDate(string input)
        {
            byte[] bytes = input.Select(c => (byte)c).ToArray();
            double oa = BitConverter.ToDouble(bytes);
            return DateTime.FromOADate(oa);
        }
    }
}
