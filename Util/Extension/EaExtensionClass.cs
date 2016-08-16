using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;

namespace hoTools.Utils.Extension
{
    /// <summary>
    /// Extension Methods for EA Repository
    /// </summary>
    public static class EaExtensionClass
    {
      
        /// <summary>
        /// Returns a list of connectors. The query has to select ea_guid (the only column) of the needed connectors.
        /// </summary>
        /// <param name="rep"></param>
        /// <param name="sql">SQL which select ea_guid</param>
        /// <returns>List of connectors</returns>
        public static List<EA.Connector> GetConnectorsBySql(this EA.Repository rep, string sql)
        {

            var l_con = new List<EA.Connector>();
            // run query into XDocument to proceed with LinQ
            string xml =rep.SQLQuery(sql);
            var x = new XDocument(XDocument.Parse(xml));

            // get ea_guid from descendants of 
            var node = from row in x.Descendants("Row").Descendants()
                         select row;

            foreach (var row in node)
            {
                EA.Connector con = rep.GetConnectorByGuid(row.Value);
                l_con.Add(con);
            }
    
            return l_con;
        }
        /// <summary>
        /// Returns a list of strings of the query with one column.
        /// </summary>
        /// <param name="rep"></param>
        /// <param name="sql">SQL which one column</param>
        /// <returns>List of strings</returns>
        public static List<string> GetStringsBySql(this EA.Repository rep, string sql)
        {

            var l_con = new List<string>();
            // run query into XDocument to proceed with LinQ
            string xml = rep.SQLQuery(sql);
            var x = new XDocument(XDocument.Parse(xml));

            // get ea_guid from descendants of 
            var node = from row in x.Descendants("Row").Descendants()
                       select row;

            foreach (var row in node)
            {
                l_con.Add(row.Value);
            }

            return l_con;
        }
       

    }
}
