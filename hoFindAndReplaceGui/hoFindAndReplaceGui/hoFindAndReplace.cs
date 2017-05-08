using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using System.Text.RegularExpressions;

namespace hoTools.Find
{
    public class FindAndReplace
    {
        EA.Repository _rep;
         EA.Package _pkg;
         string _findString = "";
         string _replaceString = "";
         string[] _taggedValueNames;
        readonly bool _isRegularExpression;
        readonly bool _isCaseSensitive;
        readonly bool _isIgnoreWhiteSpace;
        readonly FindAndReplaceItem.FieldType _searchFieldTypes;
        readonly bool _isPackageSearch;
        readonly bool _isElementSearch;
        readonly bool _isDiagramSearch;
        readonly bool _isAttributeSearch;
        readonly bool _isOperationSearch;
        readonly bool _isTagSearch;
         Regex _regExPattern;
         int _index = -1;

        // list of all items with loaded values, expected changes and values as loaded
        private readonly List<FindAndReplaceItem> _lItems = new List<FindAndReplaceItem>();

        #region Constructor
        public FindAndReplace(EA.Repository rep, EA.Package pkg, string findString, 
            string replaceString,
            bool isCaseSensitive, bool isRegularExpression, bool isIgnoreWhiteSpace,
            bool isNameSearch, bool isDescriptionSearch, bool isStereotypeSearch, bool isTagSearch,
            string taggedValueNames, 
            bool isPackageSearch, bool isElementSearch, bool isDiagramSearch,
            bool isAttributeSearch, bool isOperationSearch
            ) {
            _rep = rep;
            _pkg = pkg;
            _findString = findString;
            _replaceString = replaceString;
            _isRegularExpression = isRegularExpression;
            _isCaseSensitive = isCaseSensitive;
            _isIgnoreWhiteSpace = isIgnoreWhiteSpace;
            if (isNameSearch) _searchFieldTypes = FindAndReplaceItem.FieldType.Name;
            if (isDescriptionSearch) _searchFieldTypes = _searchFieldTypes | FindAndReplaceItem.FieldType.Description;
            if (isTagSearch) _searchFieldTypes = _searchFieldTypes | FindAndReplaceItem.FieldType.Tag;

            if (isStereotypeSearch) _searchFieldTypes = _searchFieldTypes | FindAndReplaceItem.FieldType.Stereotype;
            
            // tagged value names
            string s = taggedValueNames.Replace(' ',','); // remove blanks
            _taggedValueNames = s.Split(new Char[] { ',', ';',':',' ' }, System.StringSplitOptions.RemoveEmptyEntries);


            _isPackageSearch = isPackageSearch;
            _isElementSearch = isElementSearch;
            _isDiagramSearch = isDiagramSearch;
            _isAttributeSearch = isAttributeSearch;
            _isOperationSearch = isOperationSearch;
            _isTagSearch = isTagSearch;

            _regExPattern = PrepareRegexp();
            _index = -1;

        }
        #endregion
        #region Properties
        public string[] tagValueNames
        {
            get { return _taggedValueNames; }
            set { _taggedValueNames = value; }
        }
        public EA.Repository rep
        {
            get { return _rep; }
            set { _rep = value; }
        }
        public EA.Package pkg
        {
            get { return _pkg; }
            set { _pkg = value; }
        }
        public List<FindAndReplaceItem> l_items => _lItems;
        public string findString
        {
            get { return _findString; }
            set { _findString = value;
                  _regExPattern = PrepareRegexp();
            }
        }
      
        public string replaceString
        {
            get { return _replaceString; }
            set
            {
                if (value != _replaceString)
                {
                    _replaceString = value;
                    PrepareRegexp();
                }
            }
        }
        public Regex regexPattern => _regExPattern;
        public bool isCaseSensitive => _isCaseSensitive;
        public bool isIgnoreWhiteSpace => _isIgnoreWhiteSpace;
        public bool isRegularExpression => _isRegularExpression;
        public FindAndReplaceItem.FieldType searchFieldType => _searchFieldTypes;

        public bool isPackageSearch => _isPackageSearch;
        public bool isElementSearch => _isElementSearch;
        public bool isDiagramSearch => _isDiagramSearch;
        public bool isAttributeSearch => _isAttributeSearch;
        public bool isOperationSearch => _isOperationSearch;
        public bool isTagSearch => _isTagSearch;
        public int Index => _index;
        #endregion
        #region FindInPackageRecursive
        /// <summary>
        /// Find all choosen items beneath selected package (recursive).
        /// - Create item list (l_items)
        /// The task is delegated doRecursivePackage
        /// </summary>
        public void FindInPackageRecursive()
        {
            _index = -1;
            RecursivePackageFind.doRecursivePkg(_rep, _pkg,  this);
            if (_lItems.Count > 0) {_index = 0;}
            //else {MessageBox.Show("No found element", String.Format("{0} elements found", _l_items.Count));}
        }
        #endregion
        #region FindNext
        public void FindNext()
        {
            if (_lItems.Count == 0)
            {
                _index = -1;
                MessageBox.Show("No found element", String.Format("{0} elements found", _lItems.Count));
                return;
            }
            _index = _index + 1;
            if (_index >= _lItems.Count)
            {
                _index = _lItems.Count - 1;
                MessageBox.Show("Last element found", String.Format("{0} elements found", _lItems.Count));
            }
        }
        #endregion
        #region FindPrevious
        public void FindPrevious()
        {
            if (_lItems.Count == 0)
            {
                _index = -1;
                MessageBox.Show("No found element", String.Format("{0} elements found", _lItems.Count));
                return;
            }
            _index = _index - 1;
            if (_index < 0)
            {
                _index = 0;
                MessageBox.Show("First element found", String.Format("{0} elements found", _lItems.Count));
            }
            
        }
        #endregion
        #region LocateCurrentElement
        public void LocateCurrentElement()
        {
            if (_index < 0)
            {
                MessageBox.Show("Nothing found beneath selected package (recursive)", "No search results");
                return;
            }
            else
            {
                FindAndReplaceItem frItem = _lItems[_index];
                frItem.locate(_rep);
            }
        }
        #endregion
        #region currentItem
        public FindAndReplaceItem currentItem() 
        {
            if (this._index == 0) return null;
            return this._lItems[_index];
        }
        #endregion
        #region lastItem
        public FindAndReplaceItem lastItem()
        {
            if (l_items.Count == 0 ) return null;
            return l_items[l_items.Count-1];
        }
        #endregion

        #region Replaceitem
        /// <summary>
        /// Replace all occurences of "Search string" by "Replace String" in current selected item.
        /// </summary>
        public int ReplaceItem()
        {
            if (_index < 0) return 0;
            FindAndReplaceItem item = _lItems[_index];// get item
            item.load(_rep);
            
            // search for name
            if ((_searchFieldTypes & FindAndReplaceItem.FieldType.Name) > 0)
            {
                item.Name = ChangeString(item.Name);
            }

            // search for description
            if ((_searchFieldTypes & FindAndReplaceItem.FieldType.Description) > 0)
            {
                item.Description = ChangeString(item.Description);
                
            }
            // search for stereotype
            if ((_searchFieldTypes & FindAndReplaceItem.FieldType.Stereotype) > 0)
            {
                item.Stereotype = ChangeString(item.Stereotype);

            }



            if ((_searchFieldTypes & FindAndReplaceItem.FieldType.Tag) > 0 )
            {
                foreach (FindAndReplaceItemTag tag in item.l_itemTag)
                {
                       tag.Value = ChangeString(tag.Value);
                       tag.save();
                }
            }

            // set to changed
            item.save(_rep, _searchFieldTypes);
            return item.CountChanges;
            

            

        }
        #endregion
        #region ReplaceAll
        public void ReplaceAll()
        {
            int indexOld = _index;
            for (int i = 0; i < _lItems.Count; i= i+1 )
            {
                _index = i;
                // already changed
                if (_lItems[_index].isUpdated) continue;
                ReplaceItem();
                LocateCurrentElement();

            }
            _index = indexOld;
            LocateCurrentElement();
        }
        #endregion
        #region SetStringRtfBox
        /// <summary>
        /// Output 'from' string to rtfBox and mark all possible changes. 
        /// It returns the count of possible changes
        /// </summary>
        /// <param name="rtfBox"></param>
        /// <param name="from"></param>
        /// <returns></returns>
        public int SetRtfBoxText(RichTextBox rtfBox, string from)
        {
            int foundChanges = 0;
            rtfBox.ResetText();
            rtfBox.SelectionBackColor = Color.AliceBlue;
            //rtfBox.Text = ""; 

            int posInText = 0;
            Match match = _regExPattern.Match(from);
            while (match.Success)
            {
                foundChanges += foundChanges;
                // output not outputted text
                if ((match.Index - posInText) > 0)
                {
                    rtfBox.SelectionBackColor = Color.AliceBlue;
                    rtfBox.AppendText(from.Substring(posInText, match.Index - posInText));
                }
                rtfBox.SelectionBackColor = Color.Gold;
                rtfBox.AppendText(match.Value);
                posInText = match.Index + match.Length;
                match = match.NextMatch();

            }
            if ((from.Length - 1 - posInText) >= 0)
            {
                rtfBox.SelectionBackColor = Color.AliceBlue;
                rtfBox.SelectionBackColor = Color.AliceBlue;
                rtfBox.AppendText(from.Substring(posInText, from.Length - posInText));
            }
            return foundChanges;

        }
        #endregion
        
        #region ChangeString
        /// <summary>
        /// Change strings of items to the stored 'newString' value. 
        /// Optional you may use your own 'newString' parameter.
        /// </summary>
        /// <param name="stringToChange"></param>
        /// <param name="newString">"Change the found values to 'neString'."</param>
        /// <returns></returns>
        public string ChangeString(string stringToChange,  string newString="")
        {
            if (newString == "") newString = _replaceString;
            Match match = _regExPattern.Match(stringToChange);
            int replacedCharactersCount = 0;
            while (match.Success)
            {
                int posStartMatch = match.Index - replacedCharactersCount ;
                int lengthMatch = match.Length;
                replacedCharactersCount = replacedCharactersCount + match.Length - newString.Length;
                stringToChange = stringToChange.Remove(posStartMatch, lengthMatch);
                stringToChange = stringToChange.Insert(posStartMatch, newString);


                match = match.NextMatch();
                
            }
            return stringToChange; 
        }
        #endregion
        #region FindStringInItem
        /// <summary>
        /// Find the count of found searches in item:
        /// - add item to l_items 
        /// - update item to l_items if item is already available
        /// 
        /// </summary>
        /// <param name="object_type"></param>
        /// <param name="GUID"></param>
        public int FindStringInItem(EA.ObjectType object_type, string GUID)
        {
            int count = 0;
            FindAndReplaceItem    frItem = null;
            
           
            frItem = FindAndReplaceItem.Factory(_rep, object_type, GUID);
            count = frItem.findCount(_regExPattern, _searchFieldTypes);
            if (count > 0)
            {
                frItem.CountChanges = count;
                this.l_items.Add(frItem);
            }
           
            return count;
           
        }
        #endregion
        #region PrepareRegexp
        /// <summary>
        /// Prepare a regular expression and returns it
        /// </summary>
        /// <returns></returns>
        private Regex PrepareRegexp()
        {
            string regPattern = this.findString;
            if (this.isRegularExpression == false)
            {
                regPattern = regPattern.Replace(".", "\\.");
                regPattern = regPattern.Replace("+", "\\+");
                regPattern = regPattern.Replace("*", "\\*");
                regPattern = regPattern.Replace("?", "\\?");
                regPattern = regPattern.Replace("[", "\\[");
                regPattern = regPattern.Replace("]", "\\]");
                regPattern = regPattern.Replace("$", "\\$");
                regPattern = regPattern.Replace("{", "\\{");
                regPattern = regPattern.Replace("}", "\\}");
                regPattern = regPattern.Replace("(", "\\(");
                regPattern = regPattern.Replace(")", "\\)");
            }
            RegexOptions options = RegexOptions.Multiline;
            if (this.isIgnoreWhiteSpace) options = options | RegexOptions.IgnorePatternWhitespace;
            if (this.isCaseSensitive == false) options = options | RegexOptions.IgnoreCase;
            return new Regex(regPattern, options);
        }
        #endregion
        #region ItemShortDescription
        public string ItemShortDescription()
        {
            if (_index < 0) return "0\0";
            return string.Format(@"{0}\{1}: {2}", _index+1, _lItems.Count, _lItems[_index]);
        }
        #endregion

        
    }
}
