using System;
using System.Collections.Generic;
using System.Windows.Forms;
using EA;
using hoTools.Utils.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace hoTools.Utils.Names
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class NamesGeneratorItem
    {
        private readonly string _objectType;
        private readonly string _stereotype;
        private readonly string _sqlTopMost;
        private readonly string _sqlTopMostAlias;

        private readonly char _numberProxyChar;
        private readonly int _numberStartValue;
        private readonly string _formatString;

        public string ObjectType => _objectType;
        public string Stereotype => _stereotype;
        public string SqlTopMost => _sqlTopMost;
        public string SqlTopMostAlias => _sqlTopMostAlias;

        public char NumberProxyChar => _numberProxyChar;
        public int NumberStartValue => _numberStartValue;
        public string FormatString => _formatString;

        [JsonConstructor]
        public NamesGeneratorItem(string objectType, string stereotype, string sqlTopMost, string sqlTopMostAlias, 
                            string numberProxyChar, int numberStartValue, string formatString)
        {
            _objectType = objectType;
            _stereotype = stereotype;
            _sqlTopMost = sqlTopMost;
            _sqlTopMostAlias = sqlTopMostAlias;
            _numberProxyChar = Convert.ToChar(numberProxyChar);
            _numberStartValue = numberStartValue;
            _formatString = formatString;
        }
        /// <summary>
        /// Check if update of name or alias
        /// </summary>
        /// <returns></returns>
        public bool IsNameUpdate()
        {
            if (! String.IsNullOrWhiteSpace(_sqlTopMost)) return true;
            if (!String.IsNullOrWhiteSpace(_sqlTopMostAlias)) return false;
            MessageBox.Show("ObjectType: '{_objectType}'\r\nStereoType: '{stereotype}'", @"Autogenerate features are fault in Settings.json");
            return true;
        }
        /// <summary>
        /// Return SQL for Name or Alias
        /// </summary>
        /// <returns></returns>
        public string GetSqlTopMost()
        { 
            if (IsNameUpdate()) return _sqlTopMost;
            return _sqlTopMostAlias;
        }
        /// <summary>
        /// Check if value is according to format.
        /// </summary>
        /// <returns></returns>
        public bool IsValid(string name)
        {
            int pos = 0;
            if (name.Length != _formatString.Length) return false;
            foreach (char c in _formatString)
            {
                if (c == _numberProxyChar)
                {
                    if (!Char.IsNumber(name[pos])) return false;
                }
                else
                {
                    if (name[pos] != c) return false;

                }
                pos = pos + 1;

            }
            return true;
        }

        /// <summary>
        /// Get the number according to format from the string value. It returns -1 is was impossible to determine the number from the name.
        /// </summary>
        /// <returns></returns>
        public int GetNumber(string name)
        {
            int pos = 0;
            string sValue = "";
            if (name.Length != _formatString.Length) return -1;
            foreach (char c in _formatString)
            {
                if (c == _numberProxyChar)
                {
                    if (Char.IsNumber(name[pos]) && Char.IsNumber(_formatString[pos]))
                    {
                        sValue = sValue + name[pos];
                    }
                }
                pos = pos + 1;

            }
            return Int32.Parse(sValue);
        }
        /// <summary>
        /// Get the resulting string from format and from integer value
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public string GetString(int number)
        {
            string sValue = "";
            for (int i = _formatString.Length - 1; i >= 0; i--)
            {
                char c = _formatString[i];
                if (c == _numberProxyChar)
                {
                    int r = number % 10;
                    sValue = r + sValue;
                    number = number / 10;
                }
                else
                {
                    sValue = c + sValue;
                }

            }


            return sValue;
        }


    }


    /// <summary>
    /// Generates auto increment names for Requirements and so
    /// Settings.Json contains the generation configuration
    /// </summary>
    public class NamesGenerator
    {
        // AutoIncrement counter
        public List<NamesGeneratorItem> NameGeneratorItems { get; set; }
        public Repository Rep { get => _rep; set => _rep = value; }

        private EA.Repository _rep;
        private string _jasonFilePath;


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="rep"></param>
        /// <param name="jasonFilePath"></param>
        public NamesGenerator(EA.Repository rep, string jasonFilePath)
        {
            Rep = rep;
            _jasonFilePath = jasonFilePath;

            // use 'Deserializing Partial JSON Fragments'
            JObject search;
            try
            {
                // Read JSON
                string text = System.IO.File.ReadAllText(jasonFilePath);
                search = JObject.Parse(text);
            }
            catch (Exception e)
            {
                MessageBox.Show($@"Can't read '{jasonFilePath}'

{e}", "Can't import Auto Increment settings from Settings.json. ");
                return;
            }

            //----------------------------------------------------------------------
            // Deserialize "AutoIncrement"
            // get JSON result objects into a list

            NameGeneratorItems = (List<NamesGeneratorItem>)JasonHelper.GetConfigurationStyleItems<NamesGeneratorItem>(search, "AutoIncrement");


        }
        /// <summary>
        /// Gets the next high number for the item. This may be for Name or Alias.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public int GetNextMost(EA.Repository rep, NamesGeneratorItem item)
        {
            _rep = rep;
            return GetNextMost(item);
        }
        /// <summary>
        /// Gets the next high number for the item. This may be for Name or Alias.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public int GetNextMost(NamesGeneratorItem item)
        {
            int highNumber = -1;
        
            EA.Collection maxElements = Rep.GetElementSet(item.GetSqlTopMost(), 2);
            // no old element found
            if (maxElements.Count == 0)
            {
                highNumber = item.NumberStartValue;
            }
            else
            {
                // update to max value
                EA.Element el1 = (EA.Element)maxElements.GetAt(0);
                if (item.IsNameUpdate()) highNumber = item.GetNumber(el1.Name) + 1;
                else
                    highNumber = item.GetNumber(el1.Alias) + 1;
            }
            
            return highNumber;
        }

        public void ApplyItem(NamesGeneratorItem item)
        {
            // get next high number
            int highNumber = GetNextMost(item);

            // Get list of Elements ordered by creation date
            string stereoType = "NULL";
            if (!String.IsNullOrWhiteSpace(item.Stereotype)) stereoType = $"'{item.Stereotype}'";
            string sql = $@"
select t1.Object_ID 
from t_object t1 
where t1.object_Type = '{item.ObjectType}' AND 
      t1.stereotype  = {stereoType} 
order by t1.CreatedDate";
            EA.Collection elements = Rep.GetElementSet(sql.Trim(), 2);
            foreach (EA.Element el in elements)
            {
                bool update = false; ;
                if (item.IsNameUpdate())
                {   // Update Name
                    if (!item.IsValid(el.Name))
                    {
                        el.Name = item.GetString(highNumber);
                        update = true;
                    }
                }
                else
                {
                    // update Alias
                    if (!item.IsValid(el.Alias))
                    {
                        el.Alias = item.GetString(highNumber);
                        update = true;

                    }

                }
                if (update)
                {
                    highNumber = highNumber + 1;
                    el.Update();
                }
            }
        }

        /// <summary>
            /// Apply the naming conventions for all elements
            /// </summary>
            public void ApplyAll()
        {
            //_rep.BatchAppend = true;
            foreach (NamesGeneratorItem item in NameGeneratorItems)
            {
                ApplyItem(item);

            }
            //_rep.BatchAppend = false;
        }

        /// <summary>
        /// Create a ToolStripItem with DropDownitems for each AutoIncrement Definition in Settings.JSON.
        /// The Tag property contains the NamesGeneratorItem.
        /// </summary>
        /// <param name="insertTemplateMenuItem"></param>
        /// <param name="eventHandler"></param>
        /// <returns></returns>
        public ToolStripMenuItem GetToolStripMenu(ToolStripMenuItem insertTemplateMenuItem, EventHandler eventHandler)
        {
            // Add item of possible style as items in drop down
            foreach (NamesGeneratorItem item in NameGeneratorItems)
            {
                ToolStripMenuItem menuItem = new ToolStripMenuItem
                {
                    Text = $@"{item.FormatString} {item.ObjectType} {item.Stereotype}",
                    ToolTipText = $@"Autogenerate Format: {item.FormatString}\r\nType{item.ObjectType}\r\nStereotype {item.Stereotype}",
                    Tag = item
                };
                menuItem.Click += eventHandler;
                insertTemplateMenuItem.DropDownItems.Add(menuItem);
            }
            return insertTemplateMenuItem;

        }
    }
}
