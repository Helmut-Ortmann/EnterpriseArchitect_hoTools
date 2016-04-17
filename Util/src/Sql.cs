using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;

namespace hoTools.Utils.SQL
{

    public class UtilSql
    {
        
        EA.Repository _rep = null;
        #region Constructor
        public UtilSql(EA.Repository rep)
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
        public List<int> getAndSortEmbeddedElements(EA.Element el, string objectType, string stereotype, string direction)
        {
            var l_ports = new List<int>();

            string queryStereotype = "";
            string queryOrderBy = @" order by o.name ";
            if (stereotype != "")
            {
                queryStereotype = @" o.stereotype in ({2}) AND";
                queryOrderBy = @" order by o.stereotype {3}, o.name  ";
            }
            
            string queryObjectType = "";
            if (objectType != "") queryObjectType = @" o.object_type = '{1}'  AND ";


            string query = @"SELECT o.object_id " +
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
                l_ports.Add(Convert.ToInt32(xEle.Element("object_id").Value));
            }

            return l_ports;
            
        }
        #endregion

        #region userHasPermission
        public Boolean userHasPermission(string userGUID)
        {
         bool result  =false;
         string query = @"SELECT 'Group' As PermissionType " +
                        @"from (t_secgrouppermission p inner join t_secusergroup grp on (p.GroupID = grp.GroupID)) " +
                        @"where grp.UserID = '" + userGUID + "' " +
                        @"UNION " +
                        @"select 'User'  from t_secuserpermission p " +
                        @"where p.UserID = '" + userGUID + "' ;";

            string str = _rep.SQLQuery(query);
            XElement xelement = XElement.Parse(str);
            // something found????
            result = xelement.Descendants("Row").Count<XElement>() > 0;
            
            return result;
        }
        #endregion
        #region isConnectionAvailable
        public Boolean isConnectionAvailable(EA.Element srcEl, EA.Element trgtEl)
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
            result = xelement.Descendants("Row").Count<XElement>() > 0;

            return result;
        }
        #endregion

        #region getUsers
        /// <summary>
        /// Get users of EA element
        /// - t_secuser
        /// </summary>
         /// <returns></returns>
        public List<string> getUsers()
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
