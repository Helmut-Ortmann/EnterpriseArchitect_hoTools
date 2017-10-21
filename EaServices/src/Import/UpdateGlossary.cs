using System;
using System.Data;
using System.Collections.Generic;
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

            // Get Table of Terms of *.csv files (Type, Term, Meaning)
            DataTable dt = Excel.MakeDataTableFromCsvFile(fileName);
            // Check Data table
            if (dt.Columns.Count < 3)
            {
                MessageBox.Show($"FileName:\t{fileName}", "*.csv file needs at least 3 columns, 'Type','Term','Meaning' !!");
                return;
            }
            // Get Column names
            string col0NameTyp = dt.Columns[0].ColumnName;
            string col1NameTerm = dt.Columns[1].ColumnName;
            string col2NameMeaning = dt.Columns[2].ColumnName;
            if (!(col0NameTyp.ToLower() == "type" &&
                  col1NameTerm.ToLower() == "term" &&
                  col2NameMeaning.ToLower() == "meaning"))
            {
                MessageBox.Show(
                    $"FileName:\t{fileName}\r\nColumn0:{col0NameTyp}\r\nColumn1:{col1NameTerm}\r\nColumn2:{col2NameMeaning}",
                    "*.csv file needs at least 3 columns, 'Type','Term','Meaning' !!");
                return;
            }


            // Import to EA as Glossary (Type, Term, Meaning)
            rep.BatchAppend = true;
            // get Glossary
            var lTerms = new List<Tuple<string, string, string, short>>();
            for (int i = rep.Terms.Count - 1; i >= 0; i--)
            {
                EA.Term term = (EA.Term)rep.Terms.GetAt((short)i);
                lTerms.Add(new Tuple<string, string, string, short>(term.Type, term.Term, term.Meaning.FormatMeaning(), (short)i));
            }
            int updateCount = UpdateGlossaryTerms(rep, dt, lTerms, col0NameTyp, col1NameTerm, col2NameMeaning);
            int insertCount = AppendGlossaryTerms(rep, dt, lTerms, col0NameTyp, col1NameTerm, col2NameMeaning);
            rep.BatchAppend = false;

            MessageBox.Show($"File:\t{fileName}\r\nUpdated:\t{updateCount}\r\nInserted:\t{insertCount}\r\n",$"Glossary updated with {updateCount+ insertCount}");
        }

        /// <summary>
        /// Append Glossary terms
        /// </summary>
        /// <param name="rep"></param>
        /// <param name="dt">Table with terms to append</param>
        /// <param name="lTerms">Type, Term, Meaning</param>
        /// <param name="col0NameTyp">Column name of 'Type'</param>
        /// <param name="col1NameTerm">Column name of 'Term'</param>
        /// <param name="col2NameMeaning">Column name of 'Meaning'</param>
        private static int  AppendGlossaryTerms(Repository rep, DataTable dt, List<Tuple<string, string, string, short>> lTerms, string col0NameTyp,
            string col1NameTerm, string col2NameMeaning)
        {
// New Glossary items
            var newTerms = from DataRow termNew in dt.Rows
                where !lTerms.Any(i1 => i1.Item1 == (string) termNew[col0NameTyp] &&
                                        i1.Item2 == (string) termNew[col1NameTerm]
                )
                select new {termNew};
            // update EA Glossary
            int insertCount = 0;
            foreach (var item in newTerms)
            {
                EA.Term term = (EA.Term) rep.Terms.AddNew((string) item.termNew[col1NameTerm], "Term");
                term.Type = (string) item.termNew[col0NameTyp];
                term.Term = (string) item.termNew[col1NameTerm];
                term.Meaning = (string) item.termNew[col2NameMeaning];
                term.Update();
                insertCount += 1;
            }
            rep.Terms.Refresh();
            return insertCount;
        }

        /// <summary>
        /// Update Glossary terms
        /// </summary>
        /// <param name="rep"></param>
        /// <param name="dt">Table with terms to update</param>
        /// <param name="lTerms">Type, Term, Meaning</param>
        /// <param name="col0NameTyp">Column name of 'Type'</param>
        /// <param name="col1NameTerm">Column name of 'Term'</param>
        /// <param name="col2NameMeaning">Column name of 'Meaning'</param>
        private static int UpdateGlossaryTerms(Repository rep, DataTable dt, List<Tuple<string, string, string, short>> lTerms, 
            string col0NameTyp, 
            string col1NameTerm,
            string col2NameMeaning)
        {
// Find records to update
            var updateTerms = from DataRow termNew in dt.Rows
                from termOld in lTerms
                where (string) termNew[col0NameTyp] == termOld.Item1 &&
                      (string) termNew[col1NameTerm] == termOld.Item2 &&
                      (string) termNew[col2NameMeaning] != termOld.Item3
                select new {itemNew = termNew, Index = termOld.Item4};

            // update EA Glossary
            int updateCount = 0;
            foreach (var item in updateTerms)
            {
                EA.Term term = (EA.Term) rep.Terms.GetAt(item.Index);
                term.Meaning = (string) item.itemNew[col2NameMeaning];
                term.Update();
                updateCount += 1;
            }
            rep.Terms.Refresh();
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
