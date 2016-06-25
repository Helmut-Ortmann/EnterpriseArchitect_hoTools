using System.Windows.Forms;

namespace hoTools.Utils.Forms
{
    /// <summary>
    /// Extension Methods for Forms.Control
    /// </summary>
    public static class FormsControl
    {
        /// <summary>
        /// Set Tooltip for a control
        /// </summary>
        public static void SetTooltip(this Control control, string toolTipText)
        {
            ToolTip toolTip = new ToolTip
            {
                IsBalloon = false,
                InitialDelay = 0,
                ShowAlways = true
            };
            toolTip.SetToolTip(control, toolTipText);
        }
    }
}
