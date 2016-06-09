using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DynamicConfigurationManager.UnitTests
{
    [TestClass]
    public class AssemblyPathUnitTest
    {
        [TestMethod]
        public void AssemblyPath_ShouldPass_If_FileNotFound()
        {
            var rtn = DynamicConfigurationManager.AppSettings["assemblyPathKey1"];
            Assert.IsNull(rtn);
        }

        [TestMethod]
        public void AssemblyPath_ShouldPass_If_FileFound()
        {
            var rtn = DynamicConfigurationManager.AppSettings["assemblyPathKey2"];
            Assert.AreEqual("Success", rtn);
        }

        [TestMethod]
        public void AssemblyPathRegEx_ShouldPass_If_FileFound()
        {
            var rtn = DynamicConfigurationManager.AppSettings["assemblyPathRegExKey"];
            Assert.AreEqual("Success", rtn);
        }

        [TestMethod]
        public void AssemblyPathRegEx_ShouldPass_If_FileNotFound()
        {
            var rtn = DynamicConfigurationManager.AppSettings["assemblyPathRegExBad"];
            Assert.IsNull(rtn);
        }
    }
}