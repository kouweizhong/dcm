using System;
using System.Diagnostics;
using System.IO;

namespace DynamicConfigurationManager.ConfigMaps
{
    /// <summary>
    ///     CurrentDirectory compares the attribute value to the fully qualified path of the current directory.
    /// </summary>
    internal class CurrentDirectory : IConfigMap
    {
        /// <summary>
        ///     Determine if there is a match of the given configuration map attribute to the current
        ///     directory of the executing application.
        /// </summary>
        /// <param name="configMapAttribute">The current configMap element.</param>
        /// <returns>Return true if we found a match.</returns>
        public bool IsMatch(string configMapAttribute)
        {
            if (configMapAttribute.Length == 0) return false;
            Trace.TraceInformation($"CurrentDirectory: matching to {Environment.CurrentDirectory}");
            var fullPath = Path.GetFullPath(configMapAttribute.TrimEnd('\\'));

            return fullPath.Equals(Environment.CurrentDirectory, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}