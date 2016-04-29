using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace hoTools.Utils
{
    public static class Package
    {
        /// <summary>
        /// Get list of package ids as comma separated list
        /// </summary>
        /// <param name="rep"></param>
        /// <param name="branch"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string getBranch (EA.Repository rep, string branch, int id)
        {
            if (id > 0)
            {
                // add current package id
                if (branch == "") branch = id.ToString();
                else branch = branch + ", " + id;

                EA.Package pkg = rep.GetPackageByID(id);
                foreach (EA.Package p in pkg.Packages)
                {
                    int pkgId = p.PackageID;
                    string s = pkgId.ToString();
                    
                    branch = getBranch(rep, branch, pkgId);
                }

                
            } 
            return branch;
        }
    }
}
