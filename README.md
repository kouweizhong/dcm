#Project Name
Dynamic Configuration Manager (DCM)
##Introduction
For most applications the “out of box” functionality of the System.Configuration classes work well. However, there are situations when you need a better way to handle multiple sets of configuration data. For example, you may need a set of configuration data per environment (e.g. development, systems integration test, user acceptance test, production, contingency, training, demo, load test, etc.). A common example is a different database connection string per environment. How do you know which database connection string to load?
DCM allows you to segment your app.config file into configuration maps. Each configuration map may represent an environment and includes application key/value pairs. The application key/value pairs for configuration map is dynamically loaded based on configuration map attributes at runtime. For example, one of the DCM configuration map attributes is hostname. The hostname attribute allows you to configure a configuration map with the hostname of your server. At runtime, DCM queries the name of the server and if it matches the hostname attribute DCM loads all application configuration key/value pairs within the configuration map.

    <configMap name="User Acceptance Test Environment" hostname="MyServerName2">
      <add key="DBconnectionString" value=" Connection String to UAT" />
    </configMap>
You can even mix multiple configuration map attributes. For example, you can mix hostname and current directory attributes. In this way, DCM only loads the key/value pairs if the configuration map matches the hostname and a specific directory.
Managing application configuration data is nothing new and there are other solutions:
•	Managing differences in configuration across deployment environments http://vspimp.blogspot.com/2009/01/managing-differences-in-configuration.html
•	Managing Multiple Configuration File Environments with Pre-Build Events http://www.hanselman.com/blog/ManagingMultipleConfigurationFileEnvironmentsWithPreBuildEvents.aspx
•	Environmental overrides made it into EntLib v3 http://bloggingabout.net/blogs/olaf/archive/2007/02/18/environmental-overrides-made-it-into-entlib-v3.aspx
•	.Net StockTrader: Configuration Service for .NET Applications and WCF Services http://msdn.microsoft.com/en-us/netframework/dd164388.aspx
•	Configuration-Specific web.config Files by By Dino Esposito, April 12, 2010 http://www.drdobbs.com/visualstudio/224201017
Dynamic Configuration Manager Solution and Features
One app.config file for 
Tutorials and Samples
TBD...
Special notes
Dynamic Configuration Manager is based on Paul Haley’s articles published at: http://www.codeproject.com/KB/dotnet/EnhancedAppSettings.aspx http://www.codeproject.com/KB/dotnet/EnhancedSettings.aspx
