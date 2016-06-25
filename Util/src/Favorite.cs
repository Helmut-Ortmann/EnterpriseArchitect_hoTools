using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace hoTools.Utils.Favorites
{
    /// <summary>
    /// Handles Favorites (EA Items) with add, delete and show/search. The search to find 
    /// all favorites is defined as Resource in Util.
    /// </summary>
    public class Favorite
    {
        readonly EA.Repository _rep;
        string _xrefGuid = "";
        string _clientGuid = "";

        #region Constructor
        /// <summary>
        /// 
        /// </summary>
        /// <param name="rep"></param>
        /// <param name="clientGuid">The client GUID of the item to remember as Favorite</param>
        public  Favorite(EA.Repository rep, string clientGuid) {
            _rep = rep;
            _xrefGuid = Guid.NewGuid().ToString();
            _clientGuid = clientGuid; // Favorite GUID
        }
        public Favorite(EA.Repository rep)
        {
            _rep = rep;
            
        }
        #endregion
        #region Install Searches
        /// <summary>
        /// Installs the search to find the resources.
        /// </summary>
        /// <param name="rep"></param>
        /// <returns></returns>
        static public bool InstallSearches(EA.Repository rep)
        {
            rep.AddDefinedSearches(Resources.Strings.SearchFavorite);
            return true;
        }
        #endregion
        #region save
        public bool Save()
        {

            this.Delete();
            // insert 
            string sql = String.Format(
                         @"insert into t_xref " +
                         @"        (XrefID, Type, Client) " +
                         @" VALUES ( '{0}','Favorite', '{1}') ", 
                         _xrefGuid, _clientGuid);
            _rep.Execute(sql);
            
            return true;
        }
        #endregion
        #region delete
        public bool Delete()
        {
            // delete all old on
            string sql = String.Format(
                         @"delete from t_xref " +
                         @"where Client = '{0}'",
                         _clientGuid);
            _rep.Execute(sql);
            return true;
        }
        #endregion
        #region search
        public void Search()
        {
            
            _rep.RunModelSearch(Resources.Strings.SearchFavoriteName, "", "","");
        }
        #endregion
    }
}
