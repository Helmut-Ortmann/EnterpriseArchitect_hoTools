using hoTools.Settings;
using System.Windows.Forms;
using EAAddinFramework.Utils;


namespace hoTools.ActiveX
{
    /// <summary>
    /// Basic class for EA Addin Controls which are visualized as a tab in the Addin window. Set properties in sequence of: 
    /// <para/>- Object Tag (defined in ActiveX Control as object, meaning according to target class
    /// <para/>- AddinSettings Settings
    /// <para/>- string Release
    /// <para/>- EA.Repository Repository
    /// </summary>
    public class AddinGUI : UserControl
    {
        EA.Repository _rep;
        string _release;
        string _projectGUID;
        Model _model;
        AddinSettings _addinSettings;
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
                if (_model == null) _model = new Model(value);
                {
                    _model.Repository = value;
                }
                _projectGUID = _rep.ProjectGUID;

                
            }
            get
            {
                return _rep;
            }
        }
        #endregion
        #region Release
        public string Release
        {
            set
            {
                _release = value;
            }
            get
            {
                return _release;
            }
        }
        #endregion
        #region AddinSettings
        public AddinSettings AddinSettings
        {
            get
            {
                return _addinSettings;
            }
            set
            {
                _addinSettings = value;

             
            }
        }
        #endregion
    }
}
