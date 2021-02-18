using HiriseLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace HiriseTest
{
    [TestClass]
    public class TreeTest
    {
        private const int FolderCount = 20;
        private const int TotalFolderCount = FolderCount * FolderCount * FolderCount;

        [TestMethod]
        public void StoreItemsAsync()
        {
            using (Connector connector = InitializeConnector(out IClientSession session))
            {
                var tasks = new Task[TotalFolderCount];
                int index = 0;

                Stopwatch stopWatch = Stopwatch.StartNew();
                for (int level1 = 0; level1 < FolderCount; level1++)
                {
                    for (int level2 = 0; level2 < FolderCount; level2++)
                    {
                        for (int level3 = 0; level3 < FolderCount; level3++)
                        {
                            IItem item = connector.Tree.GetOrAddItem(new[] { level1.ToString(), level2.ToString(), level3.ToString() }, "item");
                            item.DataAsString = $@"
Bid={level1}
Ask={level2}
Last={level3}
ISIN=XS123456789
Maturity=soon
Issuer=Hirise Reality LLC
LongDescription=Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.
ShortDescription=Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.
";
                            tasks[index++] = item.StoreAsync(session).AsTask();
                        }
                    }
                }
                Task.WaitAll(tasks);
                Console.WriteLine($"Saving {TotalFolderCount} items async in the tree took {stopWatch.ElapsedMilliseconds} ms");
            }
        }

        [TestMethod]
        public void StoreItems()
        {
            using (Connector connector = InitializeConnector(out IClientSession session))
            {

                Stopwatch stopWatch = Stopwatch.StartNew();
                for (int level1 = 0; level1 < FolderCount; level1++)
                {
                    for (int level2 = 0; level2 < FolderCount; level2++)
                    {
                        for (int level3 = 0; level3 < FolderCount; level3++)
                        {
                            IItem item = connector.Tree.GetOrAddItem(new[] { level1.ToString(), level2.ToString(), level3.ToString() }, "item");
                            item.DataAsString = $@"
Bid={level1}
Ask={level2}
Last={level3}
ISIN=XS123456789
Maturity=soon
Issuer=Hirise Reality LLC
LongDescription=Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.
ShortDescription=Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.
";
                            item.StoreAsync(session).AsTask().Wait();
                        }
                    }
                }
                Console.WriteLine($"Saving {TotalFolderCount} items in the tree took {stopWatch.ElapsedMilliseconds} ms");
            }
        }

        internal static Connector InitializeConnector(out IClientSession clientSession)
        {
            var connector = new Connector();
            Stopwatch stopWatch = Stopwatch.StartNew();
            Assert.IsNotNull(connector.Tree);
            Console.WriteLine($"Initializing Tree took {stopWatch.ElapsedMilliseconds} ms");
            clientSession = connector.Login("User3", "Machine3");
            return connector;
        }
    }
}
