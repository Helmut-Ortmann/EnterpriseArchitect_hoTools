using System.Collections.Generic;
using hoTools.Utils.Configuration;

namespace hoTools.Utils.Extensions
{
    public class Extension
    {
        // List of Extensions

        // configurations as singleton
        static readonly HoToolsGlobalCfg GlobalCfg = HoToolsGlobalCfg.Instance;

        public Extension()
        {
            LoadExtensions();
        }

        #region properties

        public List<ExtensionItem> LExtensions { get; private set; }

        #endregion


        /// <summary>
        /// Load Extensions to run
        /// </summary>
        /// <returns></returns>
        // ReSharper disable once UnusedMethodReturnValue.Global
        public bool LoadExtensions()
        {
            LExtensions = new List<ExtensionItem>();
            foreach (string extensionFileName in GlobalCfg.GetExtensionListFileCompleteName())
            {
                LExtensions.Add(new ExtensionItem(extensionFileName,"",""));

            }

            return true;
        }


    }
}
