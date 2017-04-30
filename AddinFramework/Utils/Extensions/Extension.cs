using System.Collections.Generic;
using hoTools.Utils.Configuration;


// ReSharper disable once CheckNamespace
namespace AddinFramework.Utils.ExtensionsDummy
{
    public static class Extension
    {
        // List of Extensions
        private static List<ExtensionItem> lExtension;

        // configurations as singleton
        static readonly HoToolsGlobalCfg GlobalCfg = HoToolsGlobalCfg.Instance;

        #region properties

        public static List<ExtensionItem> LExtension
        {
            get { return lExtension; }
        }

        #endregion


        /// <summary>
        /// Load Extensions to run
        /// </summary>
        /// <param name="rep"></param>
        /// <returns></returns>
        public static bool LoadExtensions(EA.Repository rep)
        {
            foreach (string extensionName in GlobalCfg.GetExtensionListFileCompleteName())
            {
                lExtension.Add(new ExtensionItem(extensionName));

            }

            return true;
        }


    }
}
