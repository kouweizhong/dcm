using System.Collections.Generic;

namespace DynamicConfigurationManager
{
    internal class DynamicConfigurationSection
    {
        private List<ConfigMap> _configMaps;
        private List<GlobalSettings> _globalSettings;
    }

    internal class GlobalSettings
    {
        public string Add { get; set; }
        public string Key { get; set; }
    }

    internal class ConfigMap
    {
        public string Add { get; set; }
        public string Key { get; set; }
    }

    internal class ConfigList
    {
        public string Add { get; set; }
        public string Key { get; set; }
    }
}