using System;
using System.Data;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using EA;
using hoTools.Utils.Export;

namespace hoTools.EaServices.Import
{

    /// <summary>
    /// Update Glossary 
    /// - from *.csv
    /// </summary>
    public static class UpdateGlossary
    {
        static char TAB = '\t';

        /// <summary>
        /// Update Glossary from *.csv file (tested with Excel). It:
        /// - Updates existing terms
        /// - Append new terms
        /// 
        /// Row1 contains the Headings:
        /// - Type
        /// - Term
        /// - Meaning
        /// hoTools takes the delimiter from the Windows settings (often ';')
        /// 
        /// Suggestion:
        /// - Maintain in Excel and store as *.csv (delimiter separated columns)
        /// </summary>
        /// <param name="rep"></param>
        public static void UpdateGlossaryFromCsv(EA.Repository rep)
        {
            var openFileDialogCsv = new OpenFileDialog
            {
                Filter = @"csv files (*.csv)|*.csv",
            };
            if (openFileDialogCsv.ShowDialog() != DialogResult.OK && openFileDialogCsv.CheckFileExists) return;

            string fileName = openFileDialogCsv.FileName;
            char eaDelimiter =
                Convert.ToChar(CultureInfo.CurrentCulture.TextInfo.ListSeparator);

            // Get Table of Terms of *.csv files (Type, Term, Meaning)
            DataTable dt = Excel.MakeDataTableFromCsvFile(fileName, eaDelimiter);
            // Check Data table
            if (dt.Columns.Count < 3)
            {
                MessageBox.Show($@"FileName:{TAB}{fileName}
Columns:{TAB}{dt.Columns.Count}
Separator:{TAB}{eaDelimiter}", @"*.csv file needs at least 3 columns, 'Type','Term','Meaning' !!");
                return;
            }

            // Get Column names
            string col0NameTyp = dt.Columns[0].ColumnName;
            string col1NameTerm = dt.Columns[1].ColumnName;
            string col2NameMeaning = dt.Columns[2].ColumnName;
            if (!
                    ((col0NameTyp == "Type" && col1NameTerm == "Term") ||
                     (col0NameTyp == "Term" && col1NameTerm == "Type")) &&
                col2NameMeaning == "Meaning"
               )
            {
                MessageBox.Show(
                    $@"FileName:{TAB}{fileName}
Column0:{TAB}'{col0NameTyp}', must be 'Type' or 'Term'
Column1:{TAB}'{col1NameTerm}', must be 'Term' or 'Type'
Column2:{TAB}'{col2NameMeaning}', must be 'Meaning",
                    @"*.csv file needs at least 3 columns, 'Type','Term','Meaning' !!");
                return;
            }


            // Import to EA as Glossary (Type, Term, Meaning)
            rep.BatchAppend = true;
            // get EA Glossary Item
            int updateCount;
            int insertCount;
            try
            {
                var lEaTerms = new List<EA.Term>();
                rep.Terms.Refresh();
                for (int i = rep.Terms.Count - 1; i >= 0; i--)
                {
                    EA.Term term = (EA.Term)rep.Terms.GetAt((short)i);
                    lEaTerms.Add(term);
                }
                updateCount = UpdateGlossaryTerms(rep, dt, lEaTerms, "Type", "Term", "Meaning");
                insertCount = AppendGlossaryTerms(rep, dt, lEaTerms, "Type", "Term", "Meaning");
            }
            catch (Exception e)
            {
                MessageBox.Show($@"{e}", @"Error update glossary");
                return;
            }

            rep.BatchAppend = false;

            MessageBox.Show($@"File:{TAB}{fileName}
Updated:{TAB}{updateCount}
Inserted:{TAB}{insertCount}",$@"Glossary updated with {updateCount+ insertCount}");
        }

        /// <summary>
        /// Append Glossary terms
        /// </summary>
        /// <param name="rep"></param>
        /// <param name="dt">Table with terms to append</param>
        /// <param name="lTerms">Type, Term, Meaning</param>
        /// <param name="colNameType">Column name of 'Type'</param>
        /// <param name="colNameTerm">Column name of 'Term'</param>
        /// <param name="colNameMeaning">Column name of 'Meaning'</param>
        private static int  AppendGlossaryTerms(Repository rep, DataTable dt, List<EA.Term> lTerms, string colNameType,
            string colNameTerm, string colNameMeaning)
        {
            int insertCount = 0;
            try
            {
                // New Glossary items

                var newTerms = from DataRow termNew in dt.Rows
                    where !lTerms.Any(t =>
                        t.Type == termNew?[colNameType].ToString() &&
                        t.Term == termNew[colNameTerm].ToString()
                    )
                    select new { termNew };
                // update EA Glossary

                foreach (var item in newTerms)
                {
                    if (String.IsNullOrEmpty(item.termNew[colNameTerm].ToString())) continue;
                    if (String.IsNullOrEmpty(item.termNew[colNameType].ToString())) continue;

                    EA.Term term = (EA.Term)rep.Terms.AddNew(item.termNew[colNameTerm].ToString(), "Term");
                    term.Type = item.termNew[colNameType].ToString();
                    term.Term = item.termNew[colNameTerm].ToString();
                    term.Meaning = item.termNew[colNameMeaning].ToString();
                    term.Update();
                    insertCount += 1;
                }

                rep.Terms.Refresh();
            }
            catch (Exception e)
            {
                MessageBox.Show($@"{e}",@"Exception Append Glossary Item");
            }

            return insertCount;
        }

        /// <summary>
        /// Update Glossary terms
        /// </summary>
        /// <param name="rep"></param>
        /// <param name="dt">Table with terms to update</param>
        /// <param name="lTerms">Type, Term, Meaning</param>
        /// <param name="colNameTyp">Column name of 'Type'</param>
        /// <param name="colNameTerm">Column name of 'Term'</param>
        /// <param name="colNameMeaning">Column name of 'Meaning'</param>
        private static int UpdateGlossaryTerms(Repository rep, DataTable dt, List<EA.Term> lTerms, 
            string colNameTyp, 
            string colNameTerm,
            string colNameMeaning)
        {
            int updateCount = 0;
            try {
                // Find records to update
                var updateTerms = from DataRow termNew in dt.Rows
                    from termEa in lTerms
                    where 
                          termNew?[colNameTyp] != null &&
                          termNew[colNameTerm] != null &&
                          termNew[colNameMeaning] != null &&
                          termNew[colNameTyp].ToString() == (termEa.Type ?? "") &&
                          termNew[colNameTerm].ToString()  == (termEa.Term ?? "") &&
                          termNew[colNameMeaning].ToString() != (termEa.Meaning ?? "")
                    select new {ItemNew = termNew, ItemEa = termEa};

                // update EA Glossary
              
                foreach (var item in updateTerms)
                {
                    EA.Term term = item.ItemEa;
                    term.Meaning = item.ItemNew[colNameMeaning].ToString();
                    try
                    {
                        term.Update();
                        updateCount += 1;
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show($@"Term={item.ItemEa}
Meaning={item.ItemNew[colNameMeaning]}

{e}", @"Exception Update Glossary Item");

                    }
                }


                rep.Terms.Refresh();
            }
            catch (Exception e)
            {
                MessageBox.Show($@"{e}",@"Exception Update Glossary Items");
            }

            return updateCount;
        }

        /// <summary>
        /// Format meaning
        /// </summary>
        /// <param name="txt"></param>
        /// <returns></returns>
        private static string FormatMeaning(this string txt)
        {
            return txt.Replace("\n", "\r\n");
        }
    }
 
}
