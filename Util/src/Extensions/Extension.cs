using System.Collections.Generic;
using hoTools.Utils.Configuration;

namespace hoTools.Utils.Extensions
{
    public class Extension
    {
        // List of Extensions
        private List<ExtensionItem> lExtension;

        // configurations as singleton
        static readonly HoToolsGlobalCfg GlobalCfg = HoToolsGlobalCfg.Instance;

        public Extension()
        {
            LoadExtensions();
        }

        #region properties

        public List<ExtensionItem> LExtension
        {
            get { return lExtension; }
        }

        #endregion


        /// <summary>
        /// Load Extensions to run
        /// </summary>
        /// <param name="rep"></param>
        /// <returns></returns>
        public bool LoadExtensions()
        {
            lExtension = new List<ExtensionItem>();
            foreach (string extensionName in GlobalCfg.GetExtensionListFileCompleteName())
            {
                lExtension.Add(new ExtensionItem(extensionName));

            }

            return true;
        }


    }
}
