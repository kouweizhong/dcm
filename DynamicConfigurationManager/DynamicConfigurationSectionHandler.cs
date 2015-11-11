using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;

namespace DynamicConfigurationManager
{
    /// <summary>
    /// </summary>
    public class DynamicConfigurationSectionHandler : IConfigurationSectionHandler
    {
        public const string ConfigMapKey = "configmap";

        private readonly HashSet<string> avoidRepeatCache =
            new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);

        private readonly ConfigMaps.ConfigMapHandler configMapHandler = new ConfigMaps.ConfigMapHandler(); // Handles ConfigMap parsing
        private readonly NameValueCollection settings = new NameValueCollection();

        private int numOfHandledConfigMaps; // Count of handled ConfigMaps, if 0 then throw exception

        #region IConfigurationSectionHandler Members

        /// <summary>
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="configContext"></param>
        /// <param name="section"></param>
        /// <returns></returns>
        /// <exception cref="ConfigurationErrorsException"></exception>
        public object Create(object parent, object configContext, XmlNode section)
        {
            // Throw error if null section
            if (section == null)
                throw new ConfigurationErrorsException("The 'DynamicConfigurationManagerSection' node is not found in app.config.");

            // Parse the config to build up the settings
            ParseConfig(section);

            // Check to see if there are any "configSet={setname}" arguments on the command line, and map those value in too
            ProcessCommandLineArgs(section);

            // Check if configMaps found
            if (numOfHandledConfigMaps == 0)
                throw new ConfigurationErrorsException("Zero configMaps handled, validate configMap attribute settings and values entered.");

            // perform variable substitutions
            SubstituteVariables();

            // copy the dynamic settings to the appSettings global and export the settings to the environment with the "DCM" prefix for sub-process consumption
            MergeToAppSettingsAndExport();

            return settings;
        }

        #endregion IConfigurationSectionHandler Members

        // Check to see if there are any "configSet={setname}" arguments on the command line
        private void ProcessCommandLineArgs(XmlNode section)
        {
            foreach (XmlNode configSet in
                Environment.GetCommandLineArgs()
                    .Where(arg => arg.StartsWith("configSet=", StringComparison.OrdinalIgnoreCase))
                    .Select(arg => arg.Substring(arg.IndexOf("=", StringComparison.Ordinal) + 1))
                    .Select(setName => section.SelectSingleNode("configSet[@name=\"" + setName + "\"]"))
                    .Where(configSet => configSet != null))
            {
                ParseConfig(configSet);
                ++numOfHandledConfigMaps;
            }
        }

        // perform string substitutions of $(keyname) to their configuration value
        // i.e. process variables like
        // <add key="pingHost" value="localhost"/>
        // <add key="Arguments" value="-n 5 -w 100 $(pingHost)"/>
        private void SubstituteVariables()
        {
            var r = new Regex(@"\$\(([^\)]+)\)", RegexOptions.IgnoreCase);
            foreach (string key in settings.AllKeys)
            {
                string value = settings[key];
                if (!r.IsMatch(value))
                    continue;

                string result = value;
                foreach (Match m in r.Matches(value))
                {
                    string outer = m.Groups[0].Value; // i.e. $(HostName)
                    string inner = m.Groups[1].Value; // i.e HostName
                    if (settings[inner] != null)
                        result = result.Replace(outer, settings[inner]);
                }
                settings[key] = result;
            }
        }

        // copy the dynamic settings to the appSettings global and export the settings to the environment with the "DCM" prefix for sub-process consumption
        private void MergeToAppSettingsAndExport()
        {
            NameValueCollection appSettings = ConfigurationManager.AppSettings;
            foreach (string key in settings.AllKeys)
            {
                appSettings[key] = settings[key];
                Environment.SetEnvironmentVariable("DCM" + key, settings[key], EnvironmentVariableTarget.Process);
            }
        }

        private void ParseConfig(XmlNode currentNode)
        {
            // for each child of the current node, call the relevant processing step
            foreach (XmlNode node in currentNode.ChildNodes)
            {
                switch (node.Name.ToLower())
                {
                    case "configmap":
                        if (ParseConfigMap(node))
                        {
                            if (node.Attributes != null)
                            {
                                // break out of loop once a successful match is found with stopOnMatch="true"
                                XmlAttribute stopOnMatch = node.Attributes["stopOnMatch"];
                                if (stopOnMatch != null)
                                {
                                    bool shouldStop;
                                    if (bool.TryParse(stopOnMatch.Value, out shouldStop))
                                    {
                                        if (shouldStop)
                                            return;
                                    }
                                }
                            }
                        }
                        break;

                    case "add":
                        ParseAddNode(node);
                        break;

                    case "include":
                    case "includeset":
                        ParseIncludeSet(node);
                        break;

                    // capability to include external files
                    case "includefile":
                        ParseIncludeFile(node);
                        break;

                    case "includedb":
                        ParseIncludeDb(node);
                        break;

                    case "includeregistry":
                        ParseIncludeRegistry(node);
                        break;

                        // TODO: implement capability to create xmlFragment values, and deserialize as objects
                }
            }
        }

        private bool ParseConfigMap(XmlNode currentNode)
        {
            bool successful = configMapHandler.IsHandled(currentNode);

            // check if we want to include the children of this configMap
            if (successful)
            {
                // Increment the number of handled configMaps
                ++numOfHandledConfigMaps;

                // This is recursive, so call parseConfig
                ParseConfig(currentNode);
            }

            return successful;
        }

        private void ParseIncludeSet(XmlNode currentNode)
        {
            if (currentNode.Attributes == null)
                return;

            XmlAttribute setName = currentNode.Attributes["set"];
            if (setName == null)
                return;

            Trace.TraceInformation("Adding Set: {0}", setName.Value);
            var configSetXPath = string.Format("configSet[@name =\"{0}\"]", setName.Value);
            XmlNode configSet = currentNode.SelectSingleNode("../" + configSetXPath);
            if (configSet == null)
                configSet = currentNode.SelectSingleNode("./" + configSetXPath);

            // find the configSet specified - must have the same parent as the parent of this include node
            if (configSet == null)
                configSet = currentNode.SelectSingleNode("../../" + configSetXPath);
            if (configSet == null)
                configSet = currentNode.SelectSingleNode("../../configSets/" + configSetXPath);
            if (configSet != null)
                ParseConfig(configSet);
        }

        private void ParseIncludeFile(XmlNode currentNode)
        {
            if (currentNode.Attributes == null)
                return;

            XmlAttribute path = currentNode.Attributes["path"];
            if (path == null)
                return;

            // build absolute path from main config file's path
            // locate the config file's path
            string mainDir = Path.GetDirectoryName(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
            if (mainDir == null) return;
            string includeFile = Path.Combine(mainDir, path.Value);

            // track whick files we open and don't open it again (avoid recursion)
            if (avoidRepeatCache.Contains(includeFile))
            {
                Trace.TraceInformation("Already processed file: {0}", path.Value);
                return;
            }
            avoidRepeatCache.Add(includeFile);

            if (!File.Exists(includeFile))
            {
                Trace.TraceInformation("File does not exist: {0}", path.Value);
                return;
            }

            Trace.TraceInformation("Retrieving configuration settings from file: {0}", path.Value);

            // open the file and include it
            var xdoc = new XmlDocument();
            xdoc.Load(includeFile);

            XmlElement config = xdoc.DocumentElement;
            if (config != null)
                ParseConfig(config);
        }

        // <includeDb cxAlias="testDbAlias" query="select key, value from AppSettings where env='$(myEnv)'" />
        // cxAlias = config db alias to a connection string
        void ParseIncludeDb(XmlNode currentNode)
        {
            Dcm.CoreFeatures.GetConfigFromDb.ParseIncludeDb(currentNode, avoidRepeatCache, s => ConfigurationManager.ConnectionStrings[s], AddSetting);
        }

        private void ParseIncludeRegistry(XmlNode currentNode)
        {
            if (currentNode.Attributes == null)
                return;

            XmlAttribute keyName = currentNode.Attributes["HKLMPath"];
            if (keyName == null)
                return;

            Trace.TraceInformation("Adding settings from Registry Key: {0}", keyName.Value);

            try
            {
                // enumerate the registry key contents
                RegistryKey rk = Registry.LocalMachine.OpenSubKey(keyName.Value);
                if (rk != null)
                {
                    foreach (string key in rk.GetValueNames())
                    {
                        string value = Convert.ToString(rk.GetValue(key));
                        AddSetting(key, value);
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.TraceInformation("Failure reading registry key: '{0}' {1}", keyName.Value, ex.Message);
            }
        }

        private void ParseAddNode(XmlNode newNode)
        {
            // _newSection.AppendChild( _newSection.OwnerDocument.ImportNode( newNode, true ) );
            if (newNode.Attributes == null)
                return;

            // Check to see if there is a configuration database alias identified
            XmlAttribute xKey = newNode.Attributes["key"];
            if (xKey == null)
                return;

            string key = xKey.Value;

            // Check to see if there is a configuration database alias identified
            XmlAttribute xValue = newNode.Attributes["value"];
            if (xValue == null)
                return;

            string value = xValue.Value;

            AddSetting(key, value);
        }

        private void AddSetting(string key, string value)
        {
            // check to see if we already have an item with the same key
            if (settings.AllKeys.Any(k => k.Equals(key, StringComparison.InvariantCultureIgnoreCase)))
            {
                // found an item with the same key, so replace the value with the new value
                settings[key] = value;
                Trace.TraceInformation("Replaced: {0} = {1}", key, value);
            }
            else
            {
                // not found already, so add it
                settings.Add(key, value);
                Trace.TraceInformation("Added: {0} = {1}", key, value);
            }
        }
    }
}

