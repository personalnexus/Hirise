using HiriseLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HiriseTest
{
    [TestClass]
    public class TreeTest
    {
        private const int FolderCount = 20;

        [TestMethod]
        public void SaveItems()
        {
            using (var connector = new Connector())
            {
                IClientSession session = connector.Login("User3", "Machine3");
                for (int level1 = 0; level1 < FolderCount; level1++)
                {
                    for (int level2 = 0; level2 < FolderCount; level2++)
                    {
                        for (int level3 = 0; level3 < FolderCount; level3++)
                        {
                            IItem item = connector.Tree.GetOrAdd(new [] { level1.ToString(), level2.ToString(), level3.ToString() }, "item");
                            item.Store($"Bid={level1}\r\nAsk={level2}\r\nLast={level3}\rISIN=XS123456789\nMaturity=soon\n\nDescription=Lorem Ipsum\r\nIssuer=Hirise Reality LLC", session);
                        }
                    }
                }
            }
        }
    }
}
