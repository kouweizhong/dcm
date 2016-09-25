using System;
using System.IO;

namespace DynamicConfigurationManager.ConfigMaps
{
    /// <summary>
    ///     WebServiceBinPath compares the configuration map attribute to the bin path of the
    ///     application web host.
    /// </summary>
    internal class WebServiceBinPath : IConfigMap
    {
        /// <summary>
        ///     Determines if the configuration map attribute matches the path to the web application
        ///     bin path.
        /// </summary>
        /// <param name="configMapAttribute">The current configMap element.</param>
        /// <returns>Return true if we found a match.</returns>
        public bool IsMatch(string configMapAttribute)
        {
            var rtnValue = false;
            var assemblyPath = GetType().Assembly.CodeBase;

            assemblyPath = Path.GetDirectoryName(assemblyPath);

            assemblyPath = assemblyPath?.Replace(@"file:\", string.Empty);

            if (configMapAttribute.Length == 0) return false;
            if (assemblyPath != null)
                rtnValue = assemblyPath.Equals(configMapAttribute.TrimEnd("\\".ToCharArray()),
                    StringComparison.InvariantCultureIgnoreCase);

            return rtnValue;
        }
    }
}