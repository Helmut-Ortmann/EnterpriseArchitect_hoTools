using System;

namespace VwTools_Utils.Clipboard
{
    public static class ClipboardNoException
    {
        // during debugging sometimes an exception is thrown
        public static void SetText(string text)
        {
            try
            {
                System.Windows.Forms.Clipboard.Clear();
                System.Windows.Forms.Clipboard.SetText(text);
            }
            catch (Exception )
            {
               // do nothing
            }
           
        }
        public static void Clear()
        {
            try
            {
                System.Windows.Forms.Clipboard.Clear();
            }
            catch (Exception)
            {
                // do nothing
            }

        }
    }
}
