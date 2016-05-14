using System;
using System.Collections.Generic;
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
            KeyUp += text_KeyUp;  // handle CTRL+Z (Undo) and CTRL+Y (Redo)

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
