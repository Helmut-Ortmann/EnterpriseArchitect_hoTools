using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

using EAAddinFramework.Utils;
using hoTools.EaServices;
using hoTools.Settings;



namespace hoTools.Script
{
    /// <summary>
    /// COM Component hoTools.Script
    /// </summary>
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    [Guid("23722D72-7C6A-4246-B8B8-8D421CEBCD65")]
    [ProgId("hoTools.ScriptGUI")]
    [ComDefaultInterface(typeof(IScriptGUI))]
    public partial class ScriptGUI : UserControl, IScriptGUI
    {

        private Model _model = null;
        private String _release = null;
        private AddinSettings _addinSettings = null;

        #region Constructor
        public ScriptGUI()
        {
            InitializeComponent();
        }
        #endregion

        #region Properties
        // needs to set just after creating Control
        public Model model
        {
            set
            {
                _model = value;

            }
        }
        public string Release
        {
            set
            {
                    _release = value;
                    lbl_Release.Text = value;
            }
        }

        #region addinSettings
        public AddinSettings addinSettings
        {
            get
            {
                return _addinSettings;
            }
            set
            {
                this._addinSettings = value;

               

            }
        }
        #endregion
        #endregion

 
        // Interface IScriptGUI implementation
        public string getName() => "hoTools.ScriptGUI";
    }
}
