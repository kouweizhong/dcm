using System;
using System.Collections.Generic;
using System.Linq;

namespace DynamicConfigurationManager.ConfigMaps
{
    internal class CommandLineArgs : IConfigMap
    {
        public static IEnumerable<string> GetCommandLineArgs()
        {
            return Environment.GetCommandLineArgs();
        }

        public static IEnumerable<string> GetSetterArgs()
        {
            return (from arg in GetCommandLineArgs()
                    let varval = arg.Split("=".ToCharArray(), 2, StringSplitOptions.RemoveEmptyEntries)
                    where varval.Length == 2
                    select arg);
        }

        // handle commandLineArg="env=GUS-INIT-SIT"
        public bool Execute(string configMapAttribute)
        {
            // Make sure config attribute uses "var=value" syntax
            string[] argandvalue = configMapAttribute.Split("=".ToCharArray(), 2, StringSplitOptions.RemoveEmptyEntries);
            if (argandvalue.Length != 2)
                throw new ArgumentException("Incorrect syntax for commandLineArg match: " + configMapAttribute);
            var setterArgs = GetSetterArgs().ToArray();
            return setterArgs.Any(arg => arg.Equals(configMapAttribute, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}