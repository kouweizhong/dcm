using System;

namespace DynamicConfigurationManager.ConfigMaps
{
    /// <summary>
    /// EnvironmentVariable matches to any environment varial and value; e.g. environmentVariable="environment=INIT".
    /// </summary>
    internal class EnvironmentVariable : IConfigMap
    {
        public bool Execute(string configMapAttribute)
        {
            // Create an array of items split by '=' character, don't forget to trim the spaces.
            string[] envandvalue = configMapAttribute.Split('=');
            if (envandvalue.Length < 2)
                return false;

            string env = Environment.GetEnvironmentVariable(envandvalue[0].Trim());
            if (string.IsNullOrWhiteSpace(env))
                return false;
            return env.Equals(envandvalue[1].Trim(), StringComparison.InvariantCultureIgnoreCase);
        }
    }
}