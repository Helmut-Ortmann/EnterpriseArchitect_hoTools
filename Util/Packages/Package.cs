using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using hoTools.Utils.Sql;

namespace hoTools.Utils.Packages
{
    public class Package
    {

        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        private readonly EA.Repository _rep;
        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        private readonly EaSql _eaSql;

        /// <summary>
        /// List conflict in the component model
        /// - Components more than once on a diagram
        /// - Objects (Component, Required, Provided Interface, Port)  with redundant descriptions
        /// - Redundant interfaces (an interface name appears more than once)
        /// - Components which should be deleted '..._DeleteMe'
        /// </summary>
        private List<PackageItem> _packageItems = new List<PackageItem>();
        /// <summary>
        /// List of PackageItems to fast recursively search descendants
        /// </summary>
        public List<PackageItem> PackageItems => _packageItems;

        public Package(EA.Repository rep)
        {

            _rep = rep;



            _eaSql = new EaSql(_rep);




        }
        /// <summary>
        /// Init repository
        /// 
        /// </summary>
        public void Init()
        {
            _packageItems = new List<PackageItem>();
        }
        /// <summary>
        /// Load the package
        /// - Id
        /// - ParentId
        /// </summary>
        public bool Load()
        {
            var sql = $@"
select 
   pkg.Package_Id As Id,
   pkg.Parent_Id As ParentId
from t_package pkg
order by 1";
            var dt = _eaSql.SqlQueryDt(sql);
            _packageItems = (from row in dt.AsEnumerable()
                             select new PackageItem
                             {
                                 Id = Int32.Parse(row.Field<string>("Id")),
                                 ParentId = Int32.Parse(row.Field<string>("ParentId"))
                             }).ToList();

            return true;
        }
        /// <summary>
        /// Get the descendant ids of a given parentId using memorization
        /// </summary>
        /// <param name="packageItems"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public static List<int> GetDescendantIds(List<PackageItem> packageItems, int parentId)
        {
            Dictionary<int, List<int>> memo = new Dictionary<int, List<int>>();
            return GetDescendantIds(packageItems, parentId, memo);
        }
        /// <summary>
        ///  Recursive helper method to get the descendant ids
        /// </summary>
        /// <param name="packageItems"></param>
        /// <param name="parentId"></param>
        /// <param name="memo"></param>
        /// <returns></returns>
        private static List<int> GetDescendantIds(List<PackageItem> packageItems, int parentId, Dictionary<int, List<int>> memo)
        {
            // If the memo dictionary contains the result for the given parentId, return the cached result
            if (memo.TryGetValue(parentId, out var ids))
            {
                return ids;
            }

            List<int> descendantIds = new List<int>();

            // Iterate through the packageItems and find the descendants of the given parentId
            foreach (var item in packageItems)
            {
                if (item.ParentId == parentId)
                {
                    descendantIds.Add(item.Id);
                    descendantIds.AddRange(GetDescendantIds(packageItems, item.Id, memo));
                }
            }
            // Store the result in the memo dictionary and return it
            memo[parentId] = descendantIds;
            return descendantIds;
        }


        /// <summary>
        /// PackageItems and their children
        /// </summary>
        public class PackageItem
        {
            public int Id;
            public int ParentId;
            public List<PackageItem> Children = new List<PackageItem>();
        }
    }
}
