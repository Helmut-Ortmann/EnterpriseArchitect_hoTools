using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace hoTools.hoSqlGuis
{
    /// <summary>
    /// Text Box with UnDo/ReDo functionality by
    /// <para/>-Function: UndoText()
    /// <para/>-Function: ReText()
    /// <para/>-CTRL+Z (undo)  (don't works with EA!!)
    /// <para/>-CTRL+Y (redo)  (don't works with EA!!)
    /// </summary>
    public class TextBoxUndo : TextBox
    {
        readonly TabPage _tabPage;
        private static readonly int UndoLimit;
        readonly List<Item> _lastData = new List<Item>();
        int _undoCount;
        bool _undo;
        
        #region Constructor
        /// <summary>
        /// Constructor
        /// <para/>- Register TextChanged
        /// </summary>
        /// <param name="tabPage">The TabPage</param>
        public TextBoxUndo(TabPage tabPage)
        {
            _tabPage = tabPage;
            InitializeComponent();
        }
        #endregion

        #region UndoText
        /// <summary>
        /// Undo last change of TextBobUndo
        /// </summary>
        public void UndoText()
        {
            _undo = true;
            try
            {
                ++_undoCount;
                Text = _lastData[_lastData.Count - _undoCount - 1].Text;
                SelectionStart = _lastData[_lastData.Count - _undoCount - 1].Position;
                PerformLayout();
            }
            catch
            {
                --_undoCount;
            }

            _undo = false;
        }
        #endregion

        #region RedoText
        /// <summary>
        /// Redo last change of TextBobUndo
        /// </summary>
        public void RedoText() { 
                _undo = true;
                try
                {
                    --_undoCount;
                    Text = _lastData[_lastData.Count - _undoCount + 1].Text;
                    SelectionStart = _lastData[_lastData.Count - _undoCount + 1].Position;
                    PerformLayout();
                }
                catch
                {
                    ++_undoCount;
                }

                _undo = false;
        }
        #endregion

        #region Key up
        /// <summary>
        /// Handle CTRL sequences for CTRL+Z (Undo) and CTRL+Y (Redo)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void text_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyData == (Keys.Control | Keys.Z))
            {
                RedoText();
                e.Handled = true;
                return;
            }

            if (e.KeyData == (Keys.Control | Keys.Y))
            {
                RedoText();
                e.Handled = true;
                return;
            }
            // CTRL + A
            if (e.KeyData == (Keys.Control | Keys.A))
            {
                // select all
                SelectAll();
                //ctrlA = true;
                
                e.SuppressKeyPress = e.Handled = true;
                return;
            }

        }
        #endregion
        void text_KeyDown(object sender, KeyEventArgs e)
        {
           

            // CTRL + A
            if (e.KeyData == (Keys.Control | Keys.A))
            {
                SelectAll();
                e.Handled = true;
                e.SuppressKeyPress = true;

            }

        }
        
        #region Event TextChanged
        /// <summary>
        /// Event TextChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void textBoxUndo_TextChanged(object sender, EventArgs e)
        {

            SqlFile sqlFile = (SqlFile)_tabPage.Tag;
            sqlFile.IsChanged = true;
            _tabPage.Text = sqlFile.DisplayName;
            if (!_undo)
            {
                _lastData.RemoveRange(_lastData.Count - _undoCount, _undoCount);
                _lastData.Add(new Item(Text, SelectionStart));
                _undoCount = 0;
                if (UndoLimit != 0 && UndoLimit + 1 < _lastData.Count)
                {
                    _lastData.RemoveAt(0);
                }
            }
        }
        #endregion

        #region InitializeComponent
        /// <summary>
        /// InitializeComponent. Register Event TextChanged
        /// </summary>
        private void InitializeComponent()
        {
            Multiline = true;
            ScrollBars = ScrollBars.Both;
            AcceptsReturn = true;
            AcceptsTab = true;
            ShortcutsEnabled = true; // enable keys

            // Set WordWrap to true to allow text to wrap to the next line.
            WordWrap = true;
            Modified = false;
            Dock = DockStyle.Fill;

            TextChanged += textBoxUndo_TextChanged;
            KeyUp += text_KeyUp;  // handle CTRL+Z (Undo) and CTRL+Y (Redo)
            KeyDown += text_KeyDown;  // handle CTRL+Z (Undo) and CTRL+Y (Redo)
            //KeyPress += text_KeyPress;

        }
        #endregion

    }
    #region Item
    /// <summary>
    /// Store for a Text change:
    /// <para/>- Position of change
    /// <para/>- Text of change
    /// </summary>
    class Item
    {
        public readonly String Text;
        public readonly int Position;

        public Item(String text, int position)
        {
            Text = text;
            Position = position;
        }
    }
    #endregion Item
}
