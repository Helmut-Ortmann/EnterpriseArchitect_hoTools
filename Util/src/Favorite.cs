using System;
using EA;
using hoTools.Utils.Resources;

// ReSharper disable once CheckNamespace
namespace hoTools.Utils.Favorites
{
    /// <summary>
    /// Handles Favorites (EA Items) with add, delete and show/search. The search to find 
    /// all favorites is defined as Resource in Util.
    /// </summary>
    public class Favorite
    {
        readonly Repository _rep;
        readonly string _xrefGuid = "";
        readonly string _clientGuid = "";

        #region Constructor
        /// <summary>
        /// 
        /// </summary>
        /// <param name="rep"></param>
        /// <param name="clientGuid">The client GUID of the item to remember as Favorite</param>
        public  Favorite(Repository rep, string clientGuid) {
            _rep = rep;
            _xrefGuid = Guid.NewGuid().ToString();
            _clientGuid = clientGuid; // Favorite GUID
        }
        public Favorite(Repository rep)
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
        public static bool InstallSearches(Repository rep)
        {
            rep.AddDefinedSearches(Strings.SearchFavorite);
            return true;
        }
        #endregion
        #region save
        public bool Save()
        {

            Delete();
            // insert 
            string sql = @"insert into t_xref         (XrefID, Type, Client) " +
                         $@" VALUES ( '{_xrefGuid}','Favorite', '{_clientGuid}') ";
            _rep.Execute(sql);
            
            return true;
        }
        #endregion
        #region delete
        public bool Delete()
        {
            // delete all old on
            string sql = @"delete from t_xref where Client = '{_clientGuid}'";
            _rep.Execute(sql);
            return true;
        }
        #endregion
        #region search
        public void Search()
        {
            
            _rep.RunModelSearch(Strings.SearchFavoriteName, "", "","");
        }
        #endregion
    }
}
