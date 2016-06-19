using System.Windows.Forms;
using EA;
using EAAddinFramework.Utils;
using hoTools.Settings;

namespace hoTools.ActiveX
{
    /// <summary>
    /// Basic class for EA Addin Controls which are visualized as a tab in the Addin window. Set properties in sequence of: 
    /// <para/>- Object Tag (defined in ActiveX Control as object, meaning according to target class
    /// <para/>- AddinSettings Settings
    /// <para/>- string Release
    /// <para/>- EA.Repository Repository
    /// </summary>
    public class AddinGui : UserControl
    {
        public string Release { set; get; }
        public AddinSettings AddinSettings { get; set; }
        Repository _rep;
        // needs to set just after creating Control


        public Model Model { set; get; }

        #region Repository
        public virtual Repository Repository
        {
            set
            {
                _rep = value;
                if (Repository == null) return;
                if (Model == null) Model = new Model(value);
                {
                    Model.Repository = value;
                }
               
            }
            get
            {
                return _rep;
            }
        }
        #endregion



    }
}
