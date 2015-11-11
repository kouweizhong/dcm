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
        // <includeDb cxAlias="testDbAlias" query="select key, value from AppSettings where env='$(myEnv)'"/>
        // cxAlias = config db alias to a connection string
        internal static void ParseIncludeDb(XmlNode currentNode, HashSet<string> avoidRepeatCache, Func<string, ConnectionStringSettings> currentConnectionStrings, Action<string, string> addSetting)
        {
            if (currentNode.Attributes == null)
                return;

            // Check to see if there is a configuration database alias identified
            XmlAttribute dbAlias = currentNode.Attributes["dbAlias"];
            if (dbAlias == null)
                return;

            // Check to see if there is a configuration database alias identified
            XmlAttribute query = currentNode.Attributes["query"];
            if (query == null)
                return;

            if (avoidRepeatCache.Contains(dbAlias.Value + query.Value))
            {
                Trace.TraceInformation("Already processed database: {0} {1}", dbAlias.Value, query.Value);
                return;
            }
            avoidRepeatCache.Add(dbAlias.Value + query.Value);

            // get key/value pairs from db and add them directly
            try
            {
                ConnectionStringSettings cxSetting = currentConnectionStrings(dbAlias.Value);
                if (cxSetting == null)
                    return;

                // we have a config db! So connect to it and query for any values
                Trace.TraceInformation("Retrieving configuration settings from db: {0} {1}", cxSetting.ConnectionString, query);
                using (var cx = new SqlConnection(cxSetting.ConnectionString))
                {
                    using (SqlCommand cmd = cx.CreateCommand())
                    {
                        cmd.CommandText = query.Value;
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandTimeout = 30;

                        Trace.TraceInformation("Retrieving configuration settings");

                        cx.Open();
                        using (
                            SqlDataReader dr =
                                cmd.ExecuteReader(CommandBehavior.CloseConnection | CommandBehavior.SingleResult))
                        {
                            // iterate over the results and add them
                            while (dr.Read())
                            {
                                string key = dr[0].ToString();
                                string value = dr[1].ToString();
                                addSetting(key, value);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError("Exception reading database values: {0}", ex.Message);
            }
        }
    }
}