﻿using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DynamicConfigurationManager.UnitTests
{
    [TestClass]
    public class HostNameUnitTest
    {
        [TestMethod]
        public void HostName_ShouldPass_If_Match()
        {
            var rtn = DynamicConfigurationManager.AppSettings["hostNameKey1"];
            Assert.AreEqual("hostNameValue1", rtn);
        }

        [TestMethod]
        public void HostNameList_ShouldPass_If_Match()
        {
            var rtn = DynamicConfigurationManager.AppSettings["hostNameKey2"];
            Assert.AreEqual("hostNameValue2", rtn);
        }

        [TestMethod]
        public void HostNameList_ShouldPass_If_MatchOfSomeServiceKey()
        {
            var rtn = DynamicConfigurationManager.AppSettings["SomeService"];
            Assert.AreEqual("http://localhost/myXmlWebService/Service1.asmx", rtn);
        }

        [TestMethod]
        public void HostName_And_ExecutablePath_ShouldPass_If_Match()
        {
            var rtn = DynamicConfigurationManager.AppSettings["SomeService"];
            Assert.AreEqual("http://localhost/myXmlWebService/Service1.asmx", rtn);
        }

        [TestMethod]
        public void HostnameRegEx_ShouldPass_If_Match()
        {
            var rtn = DynamicConfigurationManager.AppSettings["hostnameRegExKey1"];
            Assert.AreEqual("hostnameRegExValue1", rtn);
        }

        [TestMethod]
        public void HostnameRegEx_ShouldPass_If_MatchWithIncludeSet()
        {
            var rtn = DynamicConfigurationManager.AppSettings["dbAlias"];
            Assert.AreEqual("testDbAlias", rtn);
        }
    }
}