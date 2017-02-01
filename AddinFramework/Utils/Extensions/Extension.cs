using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EAAddinFramework.Utils;
using hoTools.Utils.Configuration;

namespace AddinFramework.Utils.Extensions
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
