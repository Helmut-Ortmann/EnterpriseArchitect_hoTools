using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;

namespace hoLinqToSql.LinqUtils
{
    /// <summary>
    /// Found:
    /// https://stackoverflow.com/questions/11830174/how-to-flatten-tree-via-linq
    /// </summary>
    public static class LinqExtensions
    {
        /// <summary>
        /// Makes an enumeration by flattening of e.g. NodTree.
        /// An Enumeration item contains of:
        /// - Item1 The item
        /// - Item2 Level
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <param name="getChilds"></param>
        /// <returns></returns>
        public static IEnumerable<Tuple<T, int>> FlattenWithLevel<T>(
            this IEnumerable<T> items,
            Func<T, IEnumerable<T>> getChilds)
        {
            var stack = new Stack<Tuple<T, int>>();
            foreach (var item in items)
                stack.Push(new Tuple<T, int>(item, 1));

            while (stack.Count > 0)
            {
                var current = stack.Pop();
                yield return current;
                foreach (var child in getChilds(current.Item1).Reverse())
                    stack.Push(new Tuple<T, int>(child, current.Item2 + 1));
            }
        }

        /// <summary>
        /// Traverse treenodes.
        /// Example:
        ///  f = nodeFunction1.Traverse().FirstOrDefault(n => n.Text == nodeTextL3);
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        public static IEnumerable<TreeNode> Traverse(this TreeNode root)
        {
            var stack = new Stack<TreeNode>();
            stack.Push(root);
            while (stack.Count > 0)
            {
                var current = stack.Pop();
                yield return current;
                foreach (TreeNode child in current.Nodes)
                    stack.Push(child);
            }
        }
        /// <summary>
        /// Flatten an enumerable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <param name="getChildren"></param>
        /// <returns></returns>
        public static IEnumerable<T> Flatten<T>(
            this IEnumerable<T> items,
            Func<T, IEnumerable<T>> getChildren)
        {
            var stack = new Stack<T>();
            foreach (var item in items)
                stack.Push(item);

            while (stack.Count > 0)
            {
                var current = stack.Pop();
                yield return current;

                var children = getChildren(current);
                if (children == null) continue;

                foreach (var child in children)
                    stack.Push(child);
            }
        }
        /// <summary>
        /// Traverse, a general solution
        /// https://stackoverflow.com/questions/11830174/how-to-flatten-tree-via-linq
        /// Example: https://ideone.com/y7Uf0p
        /// Console.WriteLine(String.Join(", ", nodes.Traverse(n => n.Children).Select(n => n.Id)));
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="fnRecurse"></param>
        /// <returns></returns>
        public static IEnumerable<T> Traverse<T>(
            this IEnumerable<T> source,
            Func<T, IEnumerable<T>> fnRecurse)
        {
            if (source != null)
            {
                Stack<IEnumerator<T>> enumerators = new Stack<IEnumerator<T>>();
                try
                {
                    enumerators.Push(source.GetEnumerator());
                    while (enumerators.Count > 0)
                    {
                        var top = enumerators.Peek();
                        while (top.MoveNext())
                        {
                            yield return top.Current;

                            var children = fnRecurse(top.Current);
                            if (children != null)
                            {
                                top = children.GetEnumerator();
                                enumerators.Push(top);
                            }
                        }

                        enumerators.Pop().Dispose();
                    }
                }
                finally
                {
                    while (enumerators.Count > 0)
                        enumerators.Pop().Dispose();
                }
            }
        }
        /// <summary>
        /// Make a EA enumerable from an array
        /// https://sparxsystems.com/enterprise_architect_user_guide/15.2/automation/xml_format_search_data.html
        ///
        /// It is used to fill the EA Search View with xml (_rep.RunModelSearch()  ) 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <returns></returns>
        public static IEnumerable<T> ToEnumerable<T>(this T[] items)
        {
            return items;
        }

        /// <summary>
        /// Makes an EA-XML from a data table
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="guid">an optional guid so store the Search in EA (to export it)</param>
        /// <returns></returns>
        public static XElement ToEaXml(this DataTable dt, string guid = "{07646484-41F8-4FA2-8229-F52239375454}")
        {

            XElement root = new XElement("ReportViewData", new XAttribute("UID", guid));
            XElement fields = new XElement("Fields");
            root.Add(fields);
            // add columns
            foreach (DataColumn col in dt.Columns)
            {
                fields.Add(new XElement("Field", new XAttribute("name", col.ColumnName)));
            }
            XElement rows = new XElement("Rows");
            root.Add(rows);

            // over all rows
            foreach (DataRow dRow in dt.Rows)
            {
                XElement row = new XElement("Row");
                rows.Add(row);
                foreach (DataColumn col in dt.Columns)
                {
                    row.Add(new XElement("Field", new XAttribute("name", col.ColumnName), new XAttribute("value", dRow[col])));
                }

            }

            return root;
        }
        /// <summary>
        /// Makes an XML
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="rootName"></param>
        /// <returns></returns>
        public static XDocument ToXml(this DataTable dt, string rootName)
        {
            if (String.IsNullOrWhiteSpace(rootName)) rootName = "Temp";
            rootName = ReplaceForXmlNames(rootName);
            var xdoc = new XDocument

            {
                Declaration = new XDeclaration("1.0", "utf-8", "")
            };
            xdoc.Add(new XElement(rootName));
            foreach (DataRow row in dt.Rows)
            {
                if (String.IsNullOrWhiteSpace(dt.TableName)) dt.TableName = rootName;
                var element = new XElement(ReplaceForXmlNames(dt.TableName));
                foreach (DataColumn col in dt.Columns)
                {
                    element.Add(new XElement(col.ColumnName, row[col].ToString().Trim(' ')));
                }
                if (xdoc.Root != null) xdoc.Root.Add(element);
            }

            return xdoc;
        }
        /// <summary>
        /// Make string xml compatible, remove special characters
        /// </summary>
        /// <param name="xmlName"></param>
        /// <returns></returns>
        private static string ReplaceForXmlNames(string xmlName)
        {
            return xmlName
                .Replace(":", "_")
                .Replace(" ", "_")
                .Replace("->", "_to_")
                .Replace("<-", "_from_")
                .Replace("[", "_")
                .Replace("]", "_")
                .Replace("'", "_")
                .Replace(".", "_")
                .Replace("/", "_")
                .Replace("\\", "_")
                .Replace("<", "_")
                .Replace(">", "_")
                .Replace(",", "_")
                .Replace(";", "_");
        }
        /// <summary>
        /// Make an xml string from table. For every Column a table.
        /// </summary>
        /// <param name="table"></param>
        /// <param name="metaIndex"></param>
        /// <returns></returns>
        public static string ToXml(this DataTable table, int metaIndex = 0)
        {
            if (String.IsNullOrWhiteSpace(table.TableName)) table.TableName = "Temp";
            XDocument xdoc = new XDocument(
                new XElement(table.TableName,
                    from column in table.Columns.Cast<DataColumn>()
                    where column != table.Columns[metaIndex]
                    select new XElement(column.ColumnName,
                        from row in table.AsEnumerable()
                        select new XElement(ReplaceForXmlNames(row.Field<string>(metaIndex)), row[column])
                    )
                )
            );

            return xdoc.ToString();
        }
    }
}
