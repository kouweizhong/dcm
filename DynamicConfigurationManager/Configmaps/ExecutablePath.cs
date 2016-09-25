using System;
using System.Diagnostics;
using System.IO;

namespace DynamicConfigurationManager.ConfigMaps
{
    /// <summary>
    ///     ExecutablePath compares the configMapAttribute value to the fully qualified path of the
    ///     AppDomain's current base directory.
    /// </summary>
    internal class ExecutablePath : IConfigMap
    {
        /// <summary>
        ///     Determines if the given configuration map attributes matches the application base directory.
        /// </summary>
        /// <param name="configMapAttribute">The current configMap element.</param>
        /// <returns>Return true if we found a match.</returns>
        public bool IsMatch(string configMapAttribute)
        {
            if (configMapAttribute.Length == 0) return false;
            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

            Trace.TraceInformation($"ExecutablePath: matching to {baseDirectory}");

            var fullPath = Path.GetFullPath(configMapAttribute.TrimEnd('\\'));

            return fullPath.Equals(baseDirectory, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}