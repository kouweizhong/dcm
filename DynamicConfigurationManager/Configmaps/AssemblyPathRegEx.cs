namespace DynamicConfigurationManager.ConfigMaps
{
    using System.Diagnostics;
    using System.Reflection;
    using System.Text.RegularExpressions;

    internal class AssemblyPathRegEx : IConfigMap
    {
        /// <summary>
        /// </summary>
        /// <returns></returns>
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