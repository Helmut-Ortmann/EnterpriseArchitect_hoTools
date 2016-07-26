
// ReSharper disable once CheckNamespace
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
        /// <param name="description"></param>
        /// <param name="help"></param>
        public GlobalKeysConfigSearch(string searchName, string key, string modifier1, string modifier2, string modifier3, string modifier4, 
            string description, string help)
            : base(searchName, key, modifier1, modifier2, modifier3, modifier4, description, help)
        {

        }

        #region GetterSetter

        public string SearchName { get { return Id; } set { Id = value; } }

        public string SearchTerm { get; set; }


        #endregion
    }
}
