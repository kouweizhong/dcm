﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml;
using Microsoft.Win32;

namespace DynamicConfigurationManager
{
    internal class DynamicConfigurationBuilder : IConfigurationBuilder
    {
        // Helps track which files we opened and don't open it again (avoid recursion).
        private readonly HashSet<string> _avoidRepeatCache = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);

        // Loads all configuration map handlers.
        private readonly ConfigMapHandler _configMapHandler = new ConfigMapHandler();

        // A new collection of application settings.
        private NameValueCollection _dynamicConfigurationAppsettings;

        // The number of handled configuration maps.
        private int NumOfHandledConfigMaps { get; set; }

        public NameValueCollection Build(XmlNode dynamicConfigurationSection)
        {
            // A collection to store application settings
            _dynamicConfigurationAppsettings = new NameValueCollection();

            // Step 1 - GlobalSettings

            // Step 1.5 - Sub Test

            // Step 2 - Configuration Maps

            // Parse the config to build up the settings
            ParseConfig(dynamicConfigurationSection);

            // Check if configMaps found
            if (NumOfHandledConfigMaps == 0)
            {
                throw new ConfigurationErrorsException("Zero configMaps handled, validate configMap attribute settings and values entered.");
            }

            return _dynamicConfigurationAppsettings;
        }

        /// <summary>
        ///     Add key value to new application settings.
        /// </summary>
        /// <param name="key">The key attribute from the add element.</param>
        /// <param name="value">The value attribute from the add element.</param>
        private void AddSetting(string key, string value)
        {
            // check to see if we already have an item with the same key
            if (_dynamicConfigurationAppsettings.AllKeys.Any(k => k.Equals(key, StringComparison.InvariantCultureIgnoreCase)))
            {
                // found an item with the same key, so replace the value with the new value
                _dynamicConfigurationAppsettings[key] = value;
                Trace.TraceInformation($"Replaced: {key} = {value}");
            }
            else
            {
                // not found already, so add it
                _dynamicConfigurationAppsettings.Add(key, value);
                Trace.TraceInformation($"Added: {key} = {value}");
            }
        }

        /// <summary>
        ///     Parse a config map element and child elements of the current node.
        /// </summary>
        /// <param name="currentNode">The current include element we need to parse.</param>
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
                            // break out of loop once a successful match is found with stopOnMatch="true"
                            var stopOnMatch = node.Attributes?["stopOnMatch"];
                            if (stopOnMatch != null)
                            {
                                bool shouldStop;
                                if (bool.TryParse(stopOnMatch.Value, out shouldStop))
                                {
                                    if (shouldStop)
                                    {
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
                        ParseIncludeSet(node);
                        break;

                    // capability to include external files
                    case "includefile":
                        ParseIncludeFile(node);
                        break;

                    case "includeregistry":
                        ParseIncludeRegistry(node);
                        break;
                }
            }
        }

        /// <summary>
        ///     Parse each key value found in the a config map and add to new application settings.
        /// </summary>
        /// <param name="newNode">A new node to add to our application settings.</param>
        private void ParseAddNode(XmlNode newNode)
        {
            // Check to see if there is a configuration database alias identified
            var keyAttribute = newNode.Attributes?["key"];
            if (keyAttribute == null)
            {
                return;
            }

            var key = keyAttribute.Value;

            // Check to see if there is a configuration database alias identified
            var valueAttribute = newNode.Attributes["value"];
            if (valueAttribute == null)
            {
                return;
            }

            var value = valueAttribute.Value;

            AddSetting(key, value);
        }


        /// <summary>
        ///     Parse a configMap node from configuration section.
        /// </summary>
        /// <param name="currentNode">The current include element we need to parse.</param>
        /// <returns>Returns true if we find a handler for the configuration map type.</returns>
        private bool ParseConfigMap(XmlNode currentNode)
        {
            var isHandled = _configMapHandler.IsHandled(currentNode);

            // check if we want to include the children of this configMap
            if (isHandled)
            {
                // Increment the number of handled configMaps
                ++NumOfHandledConfigMaps;

                // This is recursive, so call parseConfig
                ParseConfig(currentNode);
            }

            return isHandled;
        }

        /// <summary>
        ///     Parse a configuration file from the current node.
        /// </summary>
        /// <param name="currentNode">The current include element we need to parse.</param>
        private void ParseIncludeFile(XmlNode currentNode)
        {
            var path = currentNode.Attributes?["path"];
            if (path == null)
            {
                return;
            }

            // build absolute path from main config file's path locate the config file's path
            var mainDir = Path.GetDirectoryName(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);

            if (mainDir == null)
            {
                return;
            }

            var includeFile = Path.Combine(mainDir, path.Value);

            // track which files we open and don't open it again (avoid recursion)
            if (_avoidRepeatCache.Contains(includeFile))
            {
                Trace.TraceInformation($"Already processed file: {path.Value}");
                return;
            }

            _avoidRepeatCache.Add(includeFile);

            if (!File.Exists(includeFile))
            {
                Trace.TraceInformation($"File does not exist: {path.Value}");
                return;
            }

            Trace.TraceInformation($"Retrieving configuration settings from file: {path.Value}");

            // open the file and include it
            var xdoc = new XmlDocument();
            xdoc.Load(includeFile);

            var config = xdoc.DocumentElement;
            if (config != null)
            {
                ParseConfig(config);
            }
        }

        /// <summary>
        ///     Parse the current XML node and retrieves from the value from the configuration section.
        /// </summary>
        /// <param name="currentNode">The current include element we need to parse.</param>
        private void ParseIncludeRegistry(XmlNode currentNode)
        {
            var keyName = currentNode.Attributes?["HKLMPath"];
            if (keyName == null)
            {
                return;
            }

            Trace.TraceInformation($"Adding settings from Registry Key: {keyName.Value}");

            try
            {
                // enumerate the registry key contents
                var rk = Registry.LocalMachine.OpenSubKey(keyName.Value);
                if (rk == null) return;
                foreach (var key in rk.GetValueNames())
                {
                    var value = Convert.ToString(rk.GetValue(key));
                    AddSetting(key, value);
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError($"Failure reading registry key: '{keyName.Value}' {ex.Message}");
            }
        }

        /// <summary>
        ///     Parse an include element from the configuration section.
        /// </summary>
        /// <param name="currentNode">The current include element we need to parse.</param>
        private void ParseIncludeSet(XmlNode currentNode)
        {
            if (currentNode.Attributes != null)
            {
                var setName = currentNode.Attributes["set"];
                if (setName == null)
                    return;

                Trace.TraceInformation($"Adding Set: {setName.Value}");
                var configSetXPath = $"configSet[@name =\"{setName.Value}\"]";
                var configSet = currentNode.SelectSingleNode("../" + configSetXPath);
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
        }
    }
}