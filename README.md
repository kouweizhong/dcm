#Project Name
Dynamic Configuration Manager (DCM)
##Introduction
For most applications the *out of box* functionality of the System.Configuration classes work well. However, there are situations when you need a better way to handle multiple sets of configuration data. 

For example, you may need a set of configuration data per environment (e.g. development, systems integration test, user acceptance test, production, contingency, training, demo, load test, etc.). 

Another example is the need to use a different database connection string per environment. How do you know which database connection string to load?

DCM allows you to segment your app.config file into configuration maps. Each configuration map represents an environment and includes application key/value pairs. The application key/value pairs for a configuration map is dynamically loaded based on configuration map attributes at runtime. 

For example, one of the DCM configuration map attributes is hostname. The hostname attribute allows you to configure a configuration map with the hostname of your server. At runtime, DCM queries the name of the server and if it matches the hostname attribute of your configuration map attribute, DCM loads all application configuration key/value pairs within the configuration map.

    <configMap name="User Acceptance Test Environment" hostname="MyUATServerName">
      <add key="DBconnectionString" value="Connection String to UAT" />
      <add key="AnotherKeyName" value="Keyvalue1" />
    </configMap>
    <configMap name="System Integration Test Environment" hostname="MySITServerName">
      <add key="DBconnectionString" value="Connection String to SIT" />
      <add key="AnotherKeyName" value="Keyvalue1" />
    </configMap>
You can even mix multiple configuration map attributes. For example, you can mix hostname and current directory attributes. In this way, DCM only loads the key/value pairs if the configuration map matches the hostname and a specific directory.

    <configMap name="User Acceptance Test Environment" hostname="MyUATServerName" directory="d:\apps\mynewapp\">
      <add key="DBconnectionString" value="Connection String to UAT" />
      <add key="AnotherKeyName" value="Keyvalue1" />
    </configMap>

Managing application configuration data is nothing new and there are other solutions:

- [Managing differences in configuration across deployment environments](http://vspimp.blogspot.com/2009/01/managing-differences-in-configuration.html)

- [Managing Multiple Configuration File Environments with Pre-Build Events](http://www.hanselman.com/blog/ManagingMultipleConfigurationFileEnvironmentsWithPreBuildEvents.aspx)

- [Environmental overrides made it into EntLib v3](http://bloggingabout.net/blogs/olaf/archive/2007/02/18/environmental-overrides-made-it-into-entlib-v3.aspx)

- [.Net StockTrader: Configuration Service for .NET Applications and WCF Services](http://msdn.microsoft.com/en-us/netframework/dd164388.aspx)

- [Configuration-Specific web.config Files by By Dino Esposito, April 12, 2010](http://www.drdobbs.com/visualstudio/224201017)

##Configuration Map attributes
| ConfigMap Attribute   | Description 
| -------------------   | ----------- 
| AssemblyPath          | Compares the configMapAttribute value to the fully qualified path of the application hosts current directory.
| AssemblyPathRegEx     | Compares the configMapAttribute value to the fully qualified path of the application hosts current directory using RegEx.
| CommondlineArgs       | Compares the configMapAttribute value to the submitted command line arguments entered during runtime. This is an experimental configuration map.
| CurrentDirectory      | Compares the attribute value to the fully qualified path of the current directory.
| CurrentDirectoryRegEx | Compares the attribute value to the fully qualified path of the current directory using RegEx.
| ConfigPathRegEx       | Compares the attribute value to the fully qualified path of the current configuration file using RegEx.
| EnvironmentVariables  | Matches to any environment variables and value; e.g. environmentVariable="environment=INIT".
| ExecutablePath        | Compares the configMapAttribute value to the fully qualified path of the AppDomain's current base directory.
| ExecutablePathRegEx   | Compares the attribute value to the fully qualified path of the AppDomain's current base directory using RegEx.
| Hostname              | Compares the configuration attribute value to the name of the machine executing the application.
| HostnameRegEx         | Compares a regular expression from the configuration attribute value to the name of the machine executing the application using RegEx.
| HostnameList          | Compares a list of hostname from the configuration attribute value to the name of the machine executing the application. Useful if your application runs in a multi-host production farm.
| RegistryValueRegEx    | Compares a regular expression from the configuration attribute value to the value found in the systems registry.
| SitePathRegEx         | Compares the attribute value to the fully qualified path of the Site's AppDomain Path.
| UpdateLocation        | Compares the configuration map attribute value to the fully qualified path of the click once application manifest.
| WebHostName           | Compares the configuration map attribute value to the fully qualified path of the web host URI.
| WebServiceBinPath     | Compares the configuration map attribute to the bin path of the application web host.
| WebUrl                | Compares the configuration map attribute value to the fully qualified path of the web host URI and port number.
| WebUrlList            | Compares the configuration map attribute value to the fully qualified path of the web host URI and port number.
| WebUrlRegEx           | Compares the configuration map attribute value to the fully qualified path of the web host URI and port number using RegEx.

##Tutorials and Samples
TBD

##Special notes
Dynamic Configuration Manager is based on Paul Haley original code, published at: 

[CodeProject Link 1](http://www.codeproject.com/KB/dotnet/EnhancedAppSettings.aspx)

[CodeProject Link 2](http://www.codeproject.com/KB/dotnet/EnhancedSettings.aspx)
