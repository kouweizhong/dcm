namespace DynamicConfigurationManager.ConfigMaps
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;

    /// <summary>
    /// AssemblyPath compares the configMapAttribute value to the fully qualified path of the
    /// application hosts current directory.
    /// </summary>
    internal class AssemblyPath : IConfigMap
    {
        /// <summary>
        /// Executes a search of the configMapAttribute value.
        /// </summary>
        /// <param name="configMapAttribute">
        /// The value of configMap attribute from the configuration file.
        /// </param>
        /// <returns>Returns true if configMapAttribute value equals the identified value.</returns>
        public bool Execute(string configMapAttribute)
        {
            var rtnValue = false;

            if (configMapAttribute.Length != 0)
            {
                Trace.TraceInformation("ConfigMapAttribute:{0}", configMapAttribute);

                var assemblyPath = Assembly.GetExecutingAssembly().CodeBase;
                Trace.TraceInformation("AssemblyPath: matching to {0}", assemblyPath);

                var fullPath = string.Empty;

                if (configMapAttribute.StartsWith("file:///"))
                {
                    fullPath = configMapAttribute;
                }
                else
                {
                    fullPath = Path.GetFullPath(configMapAttribute.TrimEnd('\\'));
                }

                Trace.TraceInformation("AssemblyPath: '{0}'; FullPath: '{1}'", assemblyPath, fullPath);

                rtnValue = fullPath.Equals(assemblyPath, StringComparison.InvariantCultureIgnoreCase);
            }

            return rtnValue;
        }
    }
}