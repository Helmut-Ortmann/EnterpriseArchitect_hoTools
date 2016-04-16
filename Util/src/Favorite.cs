using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace hoTools.Utils.Favorites
{
    public class Favorite
    {
        private EA.Repository _rep = null;
        private string _xref_GUID = "";
        private string _client_GUID = "";

        #region Constructor
        public  Favorite(EA.Repository rep, string clientGUID) {
            _rep = rep;
            _xref_GUID = Guid.NewGuid().ToString();
            _client_GUID = clientGUID;
        }
        public Favorite(EA.Repository rep)
        {
            _rep = rep;
            
        }
        #endregion
        static public bool installSearches(EA.Repository rep)
        {
            rep.AddDefinedSearches(Resources.Strings.SearchFavorite);
            return true;
        }
        #region save
        public bool save()
        {

            this.delete();
            // insert 
            string sql = String.Format(
                         @"insert into t_xref " +
                         @"        (XrefID, Type, Client) " +
                         @" VALUES ( '{0}','Favorite', '{1}') ", 
                         _xref_GUID, _client_GUID);
            _rep.Execute(sql);
            
            return true;
        }
        #endregion
        #region delete
        public bool delete()
        {
            // delete all old on
            string sql = String.Format(
                         @"delete from t_xref " +
                         @"where Client = '{0}'",
                         _client_GUID);
            _rep.Execute(sql);
            return true;
        }
        #endregion
        #region search
        public void search()
        {
            
            _rep.RunModelSearch(Resources.Strings.SearchFavoriteName, "", "","");
        }
        #endregion
    }
}
