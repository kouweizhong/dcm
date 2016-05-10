using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Xml;

namespace DynamicConfigurationManager
{
    internal static class GetConfigFromDb
    {
        internal static void ParseIncludeDb(XmlNode currentNode, HashSet<string> avoidRepeatCache,
            Func<string, ConnectionStringSettings> currentConnectionStrings, Action<string, string> addSetting)
        {
            // Check to see if there is a configuration database alias identified
            var dbAlias = currentNode.Attributes?["dbAlias"];
            if (dbAlias == null)
                return;

            // Check to see if there is a configuration database alias identified
            var query = currentNode.Attributes["query"];
            if (query == null)
                return;

            if (avoidRepeatCache.Contains(dbAlias.Value + query.Value))
            {
                Trace.TraceInformation($"Already processed database: {dbAlias.Value} {query.Value}");
                return;
            }
            avoidRepeatCache.Add(dbAlias.Value + query.Value);

            // get key/value pairs from db and add them directly
            try
            {
                var cxSetting = currentConnectionStrings(dbAlias.Value);
                if (cxSetting == null)
                    return;

                // we have a config db! So connect to it and query for any values
                Trace.TraceInformation(
                    $"Retrieving configuration settings from db: {cxSetting.ConnectionString} {query}");
                using (var cx = new SqlConnection(cxSetting.ConnectionString))
                {
                    using (var cmd = cx.CreateCommand())
                    {
                        cmd.CommandText = query.Value;
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandTimeout = 30;

                        Trace.TraceInformation("Retrieving configuration settings");

                        cx.Open();
                        using (
                            var dr =
                                cmd.ExecuteReader(CommandBehavior.CloseConnection | CommandBehavior.SingleResult))
                        {
                            // iterate over the results and add them
                            while (dr.Read())
                            {
                                var key = dr[0].ToString();
                                var value = dr[1].ToString();
                                addSetting(key, value);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError($"Exception reading database values: {ex.Message}");
            }
        }
    }
}