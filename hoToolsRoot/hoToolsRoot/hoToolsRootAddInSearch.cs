using hoTools.EaServices.AddInSearch;

namespace hoTools
{
    /// <summary>
    /// Partial class for Add-In Searches
    /// - https://github.com/Helmut-Ortmann/EnterpriseArchitect_hoTools/wiki/AddInModelSearch
    /// </summary>
    public partial class HoToolsRoot
    {
        /// <summary>
        /// Add-In Search to find nested Elements for:
        /// -  Selected
        /// -- Package
        /// -- Element
        /// - Comma separated GUID list of Packages or Elements in 'Search Term'  
        ///
        ///  It outputs:
        /// - All elements in it's hierarchical structure
        /// - Tagged Values
        /// 
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="searchText"></param>
        /// <param name="xmlResults"></param>
        /// <returns></returns>
        public object AddInSearchObjectsNested(EA.Repository repository, string searchText, out string xmlResults)
        {
            xmlResults = AddInSearches.SearchObjectsNested(repository, searchText);
            return "ok";
            
        }
        

    }
}
