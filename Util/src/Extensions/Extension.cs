using System.Collections.Generic;
using hoTools.Utils.Configuration;

namespace hoTools.Utils.Extensions
{
    public class Extension
    {
        // List of Extensions
        private List<ExtensionItem> _lExtensions;

        // configurations as singleton
        static readonly HoToolsGlobalCfg GlobalCfg = HoToolsGlobalCfg.Instance;

        public Extension()
        {
            LoadExtensions();
        }

        #region properties

        public List<ExtensionItem> LExtensions
        {
            get { return _lExtensions; }
        }

        #endregion


        /// <summary>
        /// Load Extensions to run
        /// </summary>
        /// <param name="rep"></param>
        /// <returns></returns>
        public bool LoadExtensions()
        {
            _lExtensions = new List<ExtensionItem>();
            foreach (string extensionFileName in GlobalCfg.GetExtensionListFileCompleteName())
            {
                _lExtensions.Add(new ExtensionItem(extensionFileName));

            }

            return true;
        }


    }
}
