using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DynamicConfigurationManager.UnitTests
{
    [TestClass]
    public class EnvironmentVariableTest
    {
        [ClassInitialize]
        public static void ClassInit(TestContext context)
        {
            // Determine whether the environment variable exists.
            if (Environment.GetEnvironmentVariable("environment") == null)
                // If it doesn't exist, create it.
                Environment.SetEnvironmentVariable("environment", "TEST");
        }

        [TestMethod]
        public void EnvironmentVariable_ShouldPass_If_Key1Found()
        {
            var rtn = DynamicConfigurationManager.AppSettings["environmentKey"];
            Assert.AreEqual("testingValue", rtn);
        }
    }
}