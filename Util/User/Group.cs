using System.Data;
using System.Linq;
using hoLinqToSql.LinqUtils;
using hoLinqToSql.LinqUtils.Extensions;

namespace hoTools.Utils.User
{
    /// <summary>
    /// Handle EA Groups
    /// </summary>
    public class EaGroup
    {
        /// <summary>
        /// Only if security is enabled
        /// Returns the groups the current user is member
        /// </summary>
        public DataTable Groups { get; }

        /// <summary>
        /// Get the groups for the current user
        /// </summary>
        /// <param name="rep"></param>
        public EaGroup(EA.Repository rep)
        {
            if (!rep.IsSecurityEnabled) 
            {
                Groups = new DataTable();
                return;
            }

            string user = rep.GetCurrentLoginUser();
            // get connection string of repository
            string connectionString = LinqUtil.GetConnectionString(rep, out var provider,out string providerName);
            using (var db = new DataModels.EaDataModel(provider, connectionString))
            {
                Groups = (from grp in db.t_secgroup
                          join grpUser in db.t_secusergroup on grp.GroupID equals grpUser.GroupID
                    join cUser in db.t_secuser on grpUser.UserID equals cUser.UserID
                          where cUser.UserLogin == user
                    orderby grp.GroupName
                    select new {Name = grp.GroupName??""}).ToDataTable();
            }
        }
    }
}
