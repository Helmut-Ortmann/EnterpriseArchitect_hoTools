using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Xml.Linq;
using EA;

namespace hoLinqToSql.LinqUtils.Extensions
{
    public static class LinqExtensions
    {
            /// <Summary>
            /// Convert a IEnumerable to a DataTable. 
            /// - Consider different "T" Types
            /// -- public Properties
            /// -- public Fields
            /// <TypeParam name="T">Type representing the type to convert.</TypeParam>
            /// <param name="source">List of requested type representing the values to convert.</param>
            /// <returns> returns a DataTable</returns>
            /// </Summary>
            public static DataTable ToDataTable<T>(this IEnumerable<T> source)
            {
                
                // ReSharper disable once PossibleMultipleEnumeration
                if (source == null || !source.Any()) return new DataTable();

                // Use reflection to get the properties/fields for the type we’re converting to a DataTable.
                var dt = new DataTable();
                var props = typeof(T).GetProperties();
                if (props.Length == 0)
                {
                    FieldInfo[] fields = typeof(T).GetFields();
                    if (fields.Length == 0)
                    {
                        MessageBox.Show($"Can't estimate fields for {typeof(T).Name}.", "Error to DataTable");
                    }
                    dt.Columns.AddRange(
                        fields.Select(p => new DataColumn(p.Name, p.FieldType.BaseType ?? typeof(System.Object))).ToArray());

                //fields.Select(p => new DataColumn(p.Name.Substring(1,p.Name.IndexOf('>')-1), p.FieldType.BaseType)).ToArray());

                    // Populate the property values to the DataTable
                    // ReSharper disable once PossibleMultipleEnumeration
                    source.ToList().ForEach(
                        i => dt.Rows.Add(fields.Select(p => p.GetValue(i)).ToArray())
                    );
                }
                else
                {
                    // Build the structure of the DataTable by converting the PropertyInfo[] into DataColumn[] using property list
                    // Add each DataColumn to the DataTable at one time with the AddRange method.
                   
                    dt.Columns.AddRange(
                        props.Select(p => new DataColumn(p.Name, Type.GetType(p.PropertyType.FullName)?? typeof(System.Object))).ToArray());
                //   props.Select(p => new DataColumn(p.Name, p.PropertyType.BaseType??typeof(System.Object))).ToArray());

                // Populate the property values to the DataTable
                // ReSharper disable once PossibleMultipleEnumeration
                source.ToList().ForEach(
                        i => dt.Rows.Add(props.Select(p => p.GetValue(i, null)).ToArray())
                    );
                }

                return dt;
            }
            
                public static DataTable ConvertToDataTable<T>(IList<T> data)
                {
                    PropertyDescriptorCollection properties =
                        TypeDescriptor.GetProperties(typeof(T));
                    DataTable table = new DataTable();
                    foreach (PropertyDescriptor prop in properties)
                        table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
                    foreach (T item in data)
                    {
                        DataRow row = table.NewRow();
                        foreach (PropertyDescriptor prop in properties)
                            row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                        table.Rows.Add(row);
                    }
                    return table;

                }
                public static DataTable ConvertListToDataTable(List<string[]> list)
                {
                    // New table.
                    DataTable table = new DataTable();

                    // Get max columns.
                    int columns = 0;
                    foreach (var array in list)
                    {
                        if (array.Length > columns)
                        {
                            columns = array.Length;
                        }
                    }

                    // Add columns.
                    for (int i = 0; i < columns; i++)
                    {
                        table.Columns.Add();
                    }

                    // Add rows.
                    foreach (var array in list)
                    {
                        table.Rows.Add(array);
                    }

                    return table;
                }
                /// <summary>
                /// Recursive collect the TreeNode and all descendants of the TreeNode (recursive).
                /// Use it with all top nodes of the TreeView.
                /// </summary>
                /// <param name="root"></param>
                /// <returns></returns>
                public static IEnumerable<TreeNode> Descendants(this TreeNode root)
                {
                    var nodes = new Stack<TreeNode>(new[] { root });
                    while (nodes.Count > 0)
                    {
                        TreeNode node = nodes.Pop();
                        yield return node;
                        foreach (TreeNode n in node.Nodes) nodes.Push(n);
                    }
                }
                /// <summary>
                /// Returns a list of strings of the query with one column.
                /// </summary>
                /// <param name="rep"></param>
                /// <param name="sql">SQL which one column</param>
                /// <returns>List of strings</returns>
                public static List<string> GetStringsBySql(this Repository rep, string sql)
                {

                    var lCon = new List<string>();
                    // run query into XDocument to proceed with LinQ
                    string xml = rep.SQLQuery(sql);
                    var x = new XDocument(XDocument.Parse(xml));

                    // get ea_guid from descendants of 
                    var node = from row in x.Descendants("Row").Descendants()
                        select row;

                    foreach (var row in node)
                    {
                        lCon.Add(row.Value);
                    }

                    return lCon;
                }
    }
    }




