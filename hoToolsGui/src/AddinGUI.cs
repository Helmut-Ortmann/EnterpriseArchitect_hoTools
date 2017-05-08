using hoTools.Settings;
using System.Windows.Forms;
using EAAddinFramework.Utils;


namespace hoTools.hoToolsGui
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
        EA.Repository _rep;
        Model _model; 
        // needs to set just after creating Control


        #region Model

      public Model Model
        {
            set
            {
                _model = value;
            }
            get
            {
                return _model;
            }
        }
        #endregion
        #region Repository
        public virtual EA.Repository Repository
        {
            set
            {
                _rep = value;
                if (Repository == null) return;
                if (_model == null) _model = new Model(value);
                {
                    _model.Repository = value;
                }
            }
            protected get
            {
                return _rep;
            }
        }
        #endregion
        #region Release
        public string Release { set; protected get; }

        #endregion
        #region AddinSettings
        public AddinSettings AddinSettings { protected get; set; }

        #endregion
    }
}
