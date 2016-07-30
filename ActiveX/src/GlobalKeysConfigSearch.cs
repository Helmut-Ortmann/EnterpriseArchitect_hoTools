
// ReSharper disable once CheckNamespace

using System.Windows.Forms;

namespace GlobalHotkeys
{
    public class GlobalKeysConfigSearch : GlobalKeysConfig
    {
        /// <summary>
        /// Constructor Global Key Definition for a Search. Id="EA Search name","relative SQL file name" or "absolute SQL file name"   Name. 
        /// </summary>
        /// <param name="searchName"></param>
        /// <param name="key"></param>
        /// <param name="modifier1"></param>
        /// <param name="modifier2"></param>
        /// <param name="modifier3"></param>
        /// <param name="modifier4"></param>
        /// <param name="help"></param>
        /// <param name="searchTerm"></param>
        public GlobalKeysConfigSearch(string key, string modifier1, string modifier2, string modifier3, string modifier4, string help,
            string searchName, string searchTerm)
            : base(id:searchName, key:key, modifier1:modifier1, modifier2:modifier2, modifier3:modifier3, modifier4:modifier4,  
                  description:"", help:help)
        {
            SearchName = searchName;
            SearchTerm = searchTerm;
        }

        #region GetterSetter

        public string SearchName { get { return Id; } set { Id = value; } }

        public string SearchTerm { get; set; }


        #endregion
    }
}
