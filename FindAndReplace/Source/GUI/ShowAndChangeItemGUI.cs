using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using hoTools.Utils;

namespace hoTools.Find
{
    public partial class ShowAndChangeItemGUI : Form
    {
        FindAndReplace _fr = null;
        FindAndReplaceItem _frItem = null;
        #region Constructor
        public ShowAndChangeItemGUI(FindAndReplace fr)
        {
            InitializeComponent();
            _fr = fr;
            _fr.LocateCurrentElement();
            ShowItem();
            ShowItem(); // I don't know why, matching at start don't lead to mark gold 
            // Element with tags
            

            txtState.Text = StateCurrentItem() + " found.";
        }
        #endregion
        #region StateCurrentItem
        private string StateCurrentItem()
        {
            chkIsChanged.Checked = _frItem.isUpdated;
             return String.Format(@"Item {0} of {1}", _fr.Index + 1, _fr.l_items.Count);
        }
        #endregion
        #region ShowItem
        /// <summary>
        /// Show the current item
        /// </summary>
        /// <param name="f"></param>
        private void ShowItem()
        {
            // fill information
            _frItem = _fr.l_items[_fr.Index];
            _frItem.load(_fr.rep);
            
            txtType.Text = _frItem.getType();
            txtSubType.Text = _frItem.getSubType();
            txtFrom.Text = _fr.findString;
            txtTo.Text = _fr.replaceString;

            // rtf fields 
            _fr.SetRtfBoxText(rtfName, _frItem.Name);
            _fr.SetRtfBoxText(rtfStereotype, _frItem.Stereotype);
            _fr.SetRtfBoxText(rtfNotes, _frItem.Description);

            if (_fr.isTagSearch)
            {
                txtTaggedValueNames.Text = string.Join(",", _fr.tagValueNames);
                txtTaggedValueNames.Visible = true;
                lblTaggedValues.Visible = true;
                gridTags.Visible = true;
                gridTags.DataSource = null;
                gridTags.AutoGenerateColumns = false;
                // load tags
                gridTags.DataSource = _frItem.l_itemTag;
            }
            else
            {
                txtTaggedValueNames.Text = "";
                txtTaggedValueNames.Visible = false;
                lblTaggedValues.Visible = false;
                gridTags.Visible = false;
            }

            txtState.Text = StateCurrentItem() + " found" ;
        }
        #endregion
        #region SaveItem
        private void SaveItem()
        {
            FindAndReplaceItem frItem = _fr.l_items[_fr.Index];
            frItem.Name = rtfName.Text;
            frItem.Description = rtfNotes.Text.Replace("\n", "\r\n");
            frItem.Stereotype = rtfStereotype.Text;
            if (frItem.isUpdated) chkIsChanged.Checked = true;
            else chkIsChanged.Checked = false;
            frItem.save(_fr.rep, FindAndReplaceItem.FieldType.Description | FindAndReplaceItem.FieldType.Name | FindAndReplaceItem.FieldType.Stereotype);
            
            // tagged values for elements
            if (_fr.isTagSearch && (gridTags != null) )
            {

                foreach (FindAndReplaceItemTag tag in frItem.l_itemTag)
                {
                    tag.save();
                }
            }
           
        }
        #endregion
        #region btnCancel_Click
        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
        #endregion

        #region btnStore_Click
        private void btnStore_Click(object sender, EventArgs e)
        {
            SaveItem();
            txtState.Text = txtState.Text = StateCurrentItem() + " stored";
        }
        #endregion
        #region btnNext_Click
        private void btnNext_Click(object sender, EventArgs e)
        {
            _fr.FindNext();
            _fr.LocateCurrentElement();
            ShowItem();
            txtState.Text = StateCurrentItem() + " next found.";
        }
        #endregion
        #region btnPrevious_Click
        private void btnPrevious_Click(object sender, EventArgs e)
        {
            _fr.FindPrevious();
            _fr.LocateCurrentElement();
            ShowItem();
            txtState.Text = StateCurrentItem() + " previous found.";
        }
        #endregion
        #region btnCancelAll_Click
        private void btnChangeAll_Click(object sender, EventArgs e)
        {
            foreach (FindAndReplaceItem frItem in _fr.l_items)
            {
                frItem.Name = _fr.ChangeString(frItem.Name, txtTo.Text);
                frItem.Description = _fr.ChangeString(frItem.Description, txtTo.Text);
                frItem.save(_fr.rep, _fr.searchFieldType);
            }

            ShowItem();
            txtState.Text = StateCurrentItem() + " all found items changed.";
        }
        #endregion
        #region btnChange_Click
        private void btnChange_Click(object sender, EventArgs e)
        {
            if (_fr.Index >= 0) { 
                FindAndReplaceItem item = _fr.l_items[_fr.Index];
                item.Name = _fr.ChangeString(item.Name,txtTo.Text);
                item.Description = _fr.ChangeString(item.Description, txtTo.Text);
                item.Stereotype = _fr.ChangeString(item.Stereotype, txtTo.Text);
                // update form
                rtfName.Text = item.Name;
                rtfNotes.Text = item.Description;
                rtfStereotype.Text = item.Stereotype;

                // 
                if (_fr.isTagSearch && (gridTags != null) )
                {
                    gridTags.DataSource = null;
                    //FindAndReplaceItemElement itemEl = (FindAndReplaceItemElement)item;
                    foreach (FindAndReplaceItemTag tag in item.l_itemTag)
                    {
                        tag.Value = _fr.ChangeString(tag.Value, txtTo.Text);
                    }
                    gridTags.AutoGenerateColumns = false;
                    gridTags.DataSource = item.l_itemTag;

                }

            }
            txtState.Text = StateCurrentItem() + " changed temporary, store if you want it permanently.";
        }
        #endregion
      }
}
