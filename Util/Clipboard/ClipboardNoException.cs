using System;
using System.Windows.Forms;

namespace hoTools.Utils.Clipboard
{
    /// <summary>
    /// Output text to Clipboard and handle Exception by MessageBox
    /// See: https://stackoverflow.com/questions/5707990/requested-clipboard-operation-did-not-succeed
    /// https://stackoverflow.com/questions/930219/how-to-handle-a-blocked-clipboard-and-other-oddities
    /// </summary>
    public static class ClipboardNoException
    {
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        static extern IntPtr CloseClipboard();
        // during debugging sometimes an exception is thrown
        public static void SetText(string text)
        {

            try
            {
                // Recommendation from: https://stackoverflow.com/questions/5707990/requested-clipboard-operation-did-not-succeed
                // https://stackoverflow.com/questions/930219/how-to-handle-a-blocked-clipboard-and-other-oddities
                // seems not always to work
                CloseClipboard();
                System.Windows.Forms.Clipboard.Clear();
                System.Windows.Forms.Clipboard.ContainsText();
                // data will remain in Clipboard after app exit, retries 20 times with 200ms delay.
                System.Windows.Forms.Clipboard.SetDataObject(String.IsNullOrWhiteSpace(text) ? $"'empty'" : $"{text}", true, 20, 200);

            }
            catch (Exception e)
            {
                return;
                MessageBox.Show($@"Just for information, from time to time

{e.ToString()}", @"Warning: Exception output text to Clipboard");
            }

        }
        /// <summary>
        /// Clear Clipboard and handle Exception
        /// </summary>
        public static void Clear()
        {
            try
            {
                System.Windows.Forms.Clipboard.Clear();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), @"Exception clear Clipboard");
            }

        }
    }
}
