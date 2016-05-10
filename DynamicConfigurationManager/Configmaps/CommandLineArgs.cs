using System;
using System.Collections.Generic;
using System.Linq;

namespace DynamicConfigurationManager.ConfigMaps
{
    /// <summary>
    ///     CommandLineArg compares the configMapAttribute value to the submitted command line arguments
    ///     entered during runtime. This is an experimental configuration map.
    /// </summary>
    internal class CommandLineArgs : IConfigMap
    {
        /// <summary>
        ///     Determine if there is a match of the given configuration map attributes to entered
        ///     command line args.
        /// </summary>
        /// <param name="configMapAttribute">The current configMap element.</param>
        /// <returns>Return true if we found a match.</returns>
        public bool Execute(string configMapAttribute)
        {
            // Make sure config attribute uses "var=value" syntax
            var argandvalue = configMapAttribute.Split("=".ToCharArray(), 2, StringSplitOptions.RemoveEmptyEntries);

            if (argandvalue.Length != 2)
            {
                throw new ArgumentException("Incorrect syntax for commandLineArg match: " + configMapAttribute);
            }

            var setterArgs = GetSetterArgs().ToArray();

            return setterArgs.Any(arg => arg.Equals(configMapAttribute, StringComparison.InvariantCultureIgnoreCase));
        }

        /// <summary>
        ///     Retrieve command line arguments entered.
        /// </summary>
        /// <returns>A string collection of command line arguments</returns>
        public static IEnumerable<string> GetCommandLineArgs()
        {
            return Environment.GetCommandLineArgs();
        }

        /// <summary>
        ///     A helper class that formats the output from GetCommandLineArg. Handle commandLineArg="env=App-INIT-SIT"
        /// </summary>
        /// <returns>A string collection of command line arguments</returns>
        public static IEnumerable<string> GetSetterArgs()
        {
            return from arg in GetCommandLineArgs()
                let varval = arg.Split("=".ToCharArray(), 2, StringSplitOptions.RemoveEmptyEntries)
                where varval.Length == 2
                select arg;
        }
    }
}