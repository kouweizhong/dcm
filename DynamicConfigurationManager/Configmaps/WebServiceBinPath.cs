using System;
using System.IO;

namespace DynamicConfigurationManager.ConfigMaps
{
    internal class WebServiceBinPath : IConfigMap
    {
        public bool Execute(string configMapAttribute)
        {
            bool rtnValue = false;
            string assemblyPath = GetType().Assembly.CodeBase;

            assemblyPath = Path.GetDirectoryName(assemblyPath);
            if (assemblyPath != null)
                assemblyPath = assemblyPath.Replace(@"file:\", "");

            if (configMapAttribute.Length != 0)
                rtnValue = assemblyPath.Equals(configMapAttribute.TrimEnd("\\".ToCharArray()),
                    StringComparison.InvariantCultureIgnoreCase);

            return rtnValue;
        }
    }
}