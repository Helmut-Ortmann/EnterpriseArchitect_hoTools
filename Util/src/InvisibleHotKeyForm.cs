using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

namespace GlobalHotkeys
{
    public partial class InvisibleHotKeyForm : Form
    {
        private const int WmActivateapp = 0x001C;
        private const int WmNcactivate = 0x0086;
        private readonly IEnumerable<Hotkey> _hotkeys;
        private readonly int _thisProcessId = Process.GetCurrentProcess().Id;
        private int _lastActiveProcessId;
        //private readonly BackgroundWorker _worker = new BackgroundWorker();

        public InvisibleHotKeyForm(IEnumerable<Hotkey> hotkeys)
        {
            InitializeComponent();
            _hotkeys = hotkeys;
            _lastActiveProcessId = _thisProcessId;
            //_worker.DoWork += worker_DoWork;
            //_worker.RunWorkerCompleted += (sender, eventArgs) => _worker.RunWorkerAsync();
            Closing += (sender, eventArgs) => ((List<Hotkey>) _hotkeys).ForEach(key => key.Dispose());
        }

        
        protected override void OnLoad(EventArgs e)
        {
            Visible = false;
            ShowInTaskbar = false;
            //_worker.RunWorkerAsync();
            base.OnLoad(e);
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == Hotkey.WmHotkeyMsgId)
            {
                foreach (Hotkey key in _hotkeys)
                {
                    if (key.IsPressedKeyCombination(m.LParam))
                    {
                        key.Handler();
                        break;
                    }
                }
            }
            else if (m.Msg == WmActivateapp || m.Msg == WmNcactivate)
            {
                bool windowGotFocus = (m.WParam.ToInt32() == 1);
                if (windowGotFocus)
                {
                    RegisterHotKeys();
                }
                else
                {
                    UnregisterHotKeys();
                }
            }
            base.WndProc(ref m);
        } 

        private void UnregisterHotKeys()
        {
            if (!_hotkeys.Any()) return;
            foreach (Hotkey key in _hotkeys)
            {
                key.Unregister();
            }
        }

        private void RegisterHotKeys()
        {
            UnregisterHotKeys();
            foreach (Hotkey key in _hotkeys)
            {
                try
                {
                    key.Register(window:this);
                }
                catch (GlobalHotkeyException exc) { Debug.WriteLine(exc.Message); }
            }
        }
    }
}
