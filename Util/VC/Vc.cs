namespace hoTools.Utils.VC
{
    public class Vc
    {
        /// <summary>
        /// EA VC states (see: http://sparxsystems.com/enterprise_architect_user_guide/13.0/automation/package_2.html)
        /// </summary>
        public enum EnumCheckOutStatus
        {
            CsUncontrolled = 0,
            CsCheckedIn,
            CsCheckedOutToThisUser,
            CsReadOnlyVersion,
            CsCheckedOutToAnotherUser,
            CsOfflineCheckedIn,
            CsCheckedOutOfflineByUser,
            CsCheckedOutOfflineByOther,
            CsDeleted,
        }
    }

}
