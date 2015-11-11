using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DynamicConfigurationManager.Tests
{
    [TestClass]
    public class GlobalAppSettingUnitTest
    {
        [TestMethod]
        public void GlobalAppSettingKey1()
        {
            string rtn = DynamicConfigurationService.AppSettings["globalKey1"];
            Assert.AreEqual("globalValue1", rtn);
        }

        [TestMethod]
        public void GlobalAppSettingKey2()
        {
            string rtn = DynamicConfigurationService.AppSettings["globalKey2"];
            Assert.AreEqual("globalValue2", rtn);
        }
    }
}