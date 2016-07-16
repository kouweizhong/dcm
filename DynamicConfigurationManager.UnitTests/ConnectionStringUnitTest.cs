using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DynamicConfigurationManager.UnitTests
{
    [TestClass]
    public class ConnectionStringUnitTest
    {
        [TestMethod]
        public void ConnectionString_ShouldPass_If_Found()
        {
            var rtn = DynamicConfigurationManager.ConnectionString("dbAlias");
            Assert.AreEqual("TestConnectionString", rtn);
        }
    }
}