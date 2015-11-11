using System;
using System.IO;
using System.Diagnostics;

namespace DynamicConfigurationManager.ConfigMaps
{
    /// <summary>
    ///     CurrentDirectory compares the attribute value to the fully qualified path of the current directory.
    /// </summary>
    internal class CurrentDirectory : IConfigMapAttribute
    {
        public bool Execute(string configMapAttribute)
        {
            bool rtnValue = false;

            if (configMapAttribute.Length != 0)
            {
                Trace.TraceInformation("CurrentDirectory: matching to {0}", Environment.CurrentDirectory);
                string fullPath = Path.GetFullPath(configMapAttribute.TrimEnd('\\'));
                rtnValue = fullPath.Equals(Environment.CurrentDirectory, StringComparison.InvariantCultureIgnoreCase);
            }

            return rtnValue;
        }
    }
}