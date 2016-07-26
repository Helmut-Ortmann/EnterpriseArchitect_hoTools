
// ReSharper disable once CheckNamespace
namespace GlobalHotkeys
{
    public class GlobalKeysSearchConfig : GlobalKeysConfig
    {
        public GlobalKeysSearchConfig(string key, string modifier1, string modifier2, string modifier3, string modifier4, string help,
            string searchName, string searchTerm)
            : base(key, modifier1, modifier2, modifier3, modifier4, help)
        {
            SearchName = searchName;
            SearchTerm = searchTerm;
        }

        #region GetterSetter

        public string SearchName { get; set; }

        public string SearchTerm { get; set; }

        public string Description
        {
            get { return SearchTerm; }
            set { SearchTerm = value; }
        }
        #endregion
    }
}
