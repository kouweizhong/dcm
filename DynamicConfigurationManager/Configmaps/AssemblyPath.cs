using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace DynamicConfigurationManager.ConfigMaps
{
    /// <summary>
    ///     AssemblyPath compares the configMapAttribute value to the fully qualified path of the
    ///     application hosts current directory.
    /// </summary>
    internal class AssemblyPath : IConfigMap
    {
        /// <summary>
        ///     Executes a search of the configMapAttribute value.
        /// </summary>
        /// <param name="configMapAttribute">
        ///     The value of configMap attribute from the configuration file.
        /// </param>
        /// <returns>Returns true if configMapAttribute value equals the identified value.</returns>
        public bool IsMatch(string configMapAttribute)
        {
            if (configMapAttribute.Length == 0) return false;
            Trace.TraceInformation($"ConfigMapAttribute:{configMapAttribute}");

            var assemblyPath = Assembly.GetExecutingAssembly().CodeBase;
            Trace.TraceInformation($"AssemblyPath: matching to {assemblyPath}");

            var fullPath = configMapAttribute.StartsWith("file:///") ? configMapAttribute : Path.GetFullPath(configMapAttribute.TrimEnd('\\'));

            Trace.TraceInformation($"AssemblyPath: '{assemblyPath}'; FullPath: '{fullPath}'");

            return fullPath.Equals(assemblyPath, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}