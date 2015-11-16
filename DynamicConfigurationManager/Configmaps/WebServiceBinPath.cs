namespace DynamicConfigurationManager.ConfigMaps
{
    using System;
    using System.IO;

    /// <summary>
    /// WebServiceBinPath compares the configuration map attribute to the bin path of the
    /// application web host.
    /// </summary>
    internal class WebServiceBinPath : IConfigMap
    {
        /// <summary>
        /// Determines if the configuration map attribute matches the path to the web application
        /// bin path.
        /// </summary>
        /// <param name="configMapAttribute">The current configMap element.</param>
        /// <returns>Return true if we found a match.</returns>
        public bool Execute(string configMapAttribute)
        {
            bool rtnValue = false;
            string assemblyPath = GetType().Assembly.CodeBase;

            assemblyPath = Path.GetDirectoryName(assemblyPath);

            if (assemblyPath != null)
            {
                assemblyPath = assemblyPath.Replace(@"file:\", string.Empty);
            }

            if (configMapAttribute.Length != 0)
            {
                rtnValue = assemblyPath.Equals(configMapAttribute.TrimEnd("\\".ToCharArray()), StringComparison.InvariantCultureIgnoreCase);
            }

            return rtnValue;
        }
    }
}