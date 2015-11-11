using System.Diagnostics;
using System.Reflection;
using System.Text.RegularExpressions;

namespace DynamicConfigurationManager.ConfigMaps
{
    internal class AssemblyPathRegEx : IConfigMapAttribute
    {
        public static string GetAssemblyPath()
        {
            return Assembly.GetExecutingAssembly().CodeBase;
        }

        public bool Execute(string configMapAttribute)
        {
            bool rtnValue = false;

            if (configMapAttribute.Length != 0)
            {
                string assemblyPath = Assembly.GetExecutingAssembly().CodeBase;
                Trace.WriteLine("AssemblyPathRegEx: matching to {0}", assemblyPath);
                var re = new Regex(configMapAttribute, RegexOptions.IgnoreCase);
                rtnValue = re.IsMatch(assemblyPath);
            }

            return rtnValue;
        }
    }
}