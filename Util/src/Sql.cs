using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using EA;

// ReSharper disable once CheckNamespace
namespace hoTools.Utils.SQL
{
    /// <summary>
    /// SQL Utilities like:
    /// - Embedded Elements
    /// - User
    /// </summary>
    public class UtilSql
    {
        readonly Repository _rep;
        #region Constructor
        public UtilSql(Repository rep)
        {
            _rep = rep;
        }
        #endregion
        #region getAndSortEmbeddedElements
        /// <summary>
        /// Get embedded elements and sort them according to name (ASC)
        /// - objectType which embedded element is selected
        /// - stereotype: Inner part of SQL in clause
        /// - direction:  If stereotype != "" then this is the sort order of the column stereotype
        /// </summary>
        /// <param name="el"></param>
        /// <param name="objectType"></param>
        /// <param name="stereotype"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        public List<int> GetAndSortEmbeddedElements(EA.Element el, string objectType, string stereotype, string direction)
        {
            var lPorts = new List<int>();

            string queryStereotype = "";
            string queryOrderBy = @" order by o.name ";
            if (stereotype != "")
            {
                queryStereotype = @" o.stereotype in ({2}) AND";
                queryOrderBy = @" order by o.stereotype {3}, o.name  ";
            }
            
            string queryObjectType = "";
            if (objectType != "") queryObjectType = @" o.object_type = '{1}'  AND ";


            string query = @"SELECT o.object_id As [object_id]" +
                           @"from t_object o " +
                           @"where " +
                           queryObjectType +
                           queryStereotype +
                           @"      o.ParentID = {0}  " +
                          queryOrderBy;
            query = String.Format(query, el.ElementID, objectType, stereotype, direction);
                          

            string str = _rep.SQLQuery(query);
            XElement xelement = XElement.Parse(str);
            foreach (XElement xEle in xelement.Descendants("Row"))
            {
                lPorts.Add(Convert.ToInt32(xEle.Element("OBJECT_ID").Value));
            }

            return lPorts;
            
        }
        #endregion

        #region userHasPermission
        public Boolean UserHasPermission(string userGuid)
        {
         bool result  =false;
         string query = @"SELECT 'Group' As PermissionType " +
                        @"from (t_secgrouppermission p inner join t_secusergroup grp on (p.GroupID = grp.GroupID)) " +
                        @"where grp.UserID = '" + userGuid + "' " +
                        @"UNION " +
                        @"select 'User'  from t_secuserpermission p " +
                        @"where p.UserID = '" + userGuid + "' ;";

            string str = _rep.SQLQuery(query);
            XElement xelement = XElement.Parse(str);
            // something found????
            result = xelement.Descendants("Row").Count() > 0;
            
            return result;
        }
        #endregion
        #region isConnectionAvailable
        public Boolean IsConnectionAvailable(EA.Element srcEl, EA.Element trgtEl)
        {
            bool result = false;
            string sql = "SELECT Start_Object_ID  " +
                         " From t_connector " +
                         " where Start_Object_ID in ( {0},{1} ) AND " +
                         "       End_Object_ID in  ( {0},{1} ) ";
             string query = String.Format(sql, srcEl.ElementID, trgtEl.ElementID);

            
            string str = _rep.SQLQuery(query);
            XElement xelement = XElement.Parse(str);
            // something found????
            result = xelement.Descendants("Row").Count() > 0;

            return result;
        }
        #endregion

        #region getUsers
        /// <summary>
        /// Get users of EA element
        /// - t_secuser
        /// </summary>
         /// <returns></returns>
        public List<string> GetUsers()
        {
            var l = new List<string>();
            string query;
            if (_rep.IsSecurityEnabled)
            {
                // authors under security
                query = @"select UserLogin As [User] from t_secuser order by 1";
            }
            else {
                // all used authors
                query = @"select distinct Author As [User] from t_object order by 1";
            }
            string str = _rep.SQLQuery(query);
            XElement xelement = XElement.Parse(str);
            foreach (XElement xEle in xelement.Descendants("Row"))
            {
                l.Add(xEle.Element("User").Value);
            }

            return l;
        }
        #endregion
    }
}
