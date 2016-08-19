using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace AddinFramework.Extension
{
    public static class EaExtensionClass
    {
        public static List<EA.Connector> GetConnectorsBySql(this EA.Repository rep, string sql)
        {

            // run query into XDocument to proceed with LinQ
            string xml =rep.SQLQuery(sql);
            var x = new XDocument(XDocument.Parse(xml));

            // get scriptID
            var rowsNode = x.Descendants("Row");
            foreach (var row in rowsNode)
            {
                string ea_guid = row.Attribute("value").Value;
            }
    
            return null;
        }

        /// <summary>
        /// get EA Release as string in the form: "9","10","11","12","12.1","13"
        /// </summary>
        /// <param name="rep"></param>
        /// <returns>EA Version</returns>
        public static string getRelease(this EA.Repository rep)
        {
            int libraryVersion = rep.LibraryVersion;
            if (libraryVersion > 1200 && libraryVersion < 1300) return "12.1";
           
            return Convert.ToString((int)libraryVersion/100);
        }
    }
}
