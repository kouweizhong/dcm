namespace DynamicConfigurationManager.ConfigMaps
{
    using System;
    using System.Diagnostics;
    using System.IO;

    /// <summary>
    /// ExecutablePath compares the configMapAttribute value to the fully qualified path of the
    /// AppDomain's current base directory.
    /// </summary>
    internal class ExecutablePath : IConfigMap
    {
        /// <summary>
        /// Determines if the given configuration map attributes matches the application base directory.
        /// </summary>
        /// <param name="configMapAttribute">The current configMap element.</param>
        /// <returns>Return true if we found a match.</returns>
        public bool Execute(string configMapAttribute)
        {
            bool rtnValue = false;

            if (configMapAttribute.Length != 0)
            {
                string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

                Trace.TraceInformation("ExecutablePath: matching to {0}", baseDirectory);

                string fullPath = Path.GetFullPath(configMapAttribute.TrimEnd('\\'));

                rtnValue = fullPath.Equals(baseDirectory, StringComparison.InvariantCultureIgnoreCase);
            }

            return rtnValue;
        }
    }
}