using Microsoft.VisualStudio.TestTools.UnitTesting;
using HiriseLib;
using System;

namespace HiriseTest
{
    [TestClass]
    [TestCategory("Local")]
    public class ConnectionTest
    {
        [TestMethod]
        public void LastConnect()
        {
            using (var connection = new Connection())
            {
                Assert.IsTrue(connection.LastConnect <= DateTime.UtcNow);
            }
        }
    }
}
