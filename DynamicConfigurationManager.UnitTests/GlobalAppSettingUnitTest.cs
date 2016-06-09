using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DynamicConfigurationManager.UnitTests
{
    [TestClass]
    public class GlobalAppSettingUnitTest
    {
        [TestMethod]
        public void GlobalAppSetting_ShouldPass_If_Key1Found()
        {
            var rtn = DynamicConfigurationManager.AppSettings["globalKey1"];
            Assert.AreEqual("globalValue1", rtn);
        }

        [TestMethod]
        public void GlobalAppSetting_ShouldPass_If_Key2Found()
        {
            var rtn = DynamicConfigurationManager.AppSettings["globalKey2"];
            Assert.AreEqual("globalValue2", rtn);
        }
    }
}