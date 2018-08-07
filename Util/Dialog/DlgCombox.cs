using System;
using System.Data;
using System.Windows.Forms;

namespace hoTools.Utils.Dialog
{
    public class DlgComboBox
    {
        public static string DialogCombo(string formCaption, DataTable comboSource, string displyMember, string valueMember, string text = "")
        {

            Form prompt = new Form
            {
                Width = 500,
                Height = 200,
                Text = formCaption
            };
            //prompt.RightToLeft = RightToLeft.Yes;

            ComboBox combo = new ComboBox
            {
                Left = 50,
                Top = 35,
                Width = 400,
                DataSource = comboSource,
                ValueMember = valueMember,
                DisplayMember = displyMember
            };
            Button confirmation = new Button() { Text = @"ok", Left = 300, Width = 50, Top = 90 };
            Button cancellation = new Button() { Text = @"cancel", Left = 370, Width = 50, Top = 90 };
            bool cancel = true;
            confirmation.Click += (sender, e) =>
            {
                cancel = false;
                prompt.Close(); };
            cancellation.Click += (sender, e) => { prompt.Close(); };
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(cancellation);
            if (!String.IsNullOrWhiteSpace(text))
            {
                Label textLabel = new Label() { Left = 50, Top = 10, Text = text };
                prompt.Controls.Add(textLabel);
            }
            prompt.Controls.Add(combo);
            prompt.ShowDialog();

            if (cancel) return "";
            else return combo.SelectedValue.ToString();
        }
    }
}
