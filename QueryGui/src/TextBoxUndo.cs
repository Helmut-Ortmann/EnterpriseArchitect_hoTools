using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace hoTools.Query
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
        TabPage _tabPage;
        static int UNDO_LIMIT;
        List<Item> LastData = new List<Item>();
        int undoCount = 0;
        bool undo;

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
            undo = true;
            try
            {
                ++undoCount;
                Text = LastData[LastData.Count - undoCount - 1].text;
                SelectionStart = LastData[LastData.Count - undoCount - 1].position;
                PerformLayout();
            }
            catch
            {
                --undoCount;
            }

            undo = false;
        }
        #endregion

        #region RedoText
        /// <summary>
        /// Redo last change of TextBobUndo
        /// </summary>
        public void RedoText() { 
                undo = true;
                try
                {
                    --undoCount;
                    Text = LastData[LastData.Count - undoCount + 1].text;
                    SelectionStart = LastData[LastData.Count - undoCount + 1].position;
                    PerformLayout();
                }
                catch
                {
                    ++undoCount;
                }

                undo = false;
        }
        #endregion
        #region Override ProcessCmdKey
        /// <summary>
        /// Override ProcessCmdKey. Handle CTRL+Z (undo) and CTRL+Y (redo).
        /// <para/> Don't works in the context of EA!
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="keyData"></param>
        /// <returns></returns>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            
            if (keyData == (Keys.Control | Keys.Z))
            {
                UndoText();
                return true;
            }

            if (keyData == (Keys.Control | Keys.Y))
            {
                RedoText();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        #endregion

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
            if (!undo)
            {
                LastData.RemoveRange(LastData.Count - undoCount, undoCount);
                LastData.Add(new Item(Text, SelectionStart));
                undoCount = 0;
                if (UNDO_LIMIT != 0 && UNDO_LIMIT + 1 < LastData.Count)
                {
                    LastData.RemoveAt(0);
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

            // Set WordWrap to true to allow text to wrap to the next line.
            WordWrap = true;
            Modified = false;
            Dock = DockStyle.Fill;

            TextChanged += textBoxUndo_TextChanged;
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
        public String text;
        public int position;

        public Item(String text, int position)
        {
            this.text = text;
            this.position = position;
        }
    }
    #endregion Item
}
