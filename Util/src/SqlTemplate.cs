using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace hoTools.Utils.SQL
{
    /// <summary>
    /// Templates to insert into SQL:
    /// - Template Text
    /// - Template Name
    /// - Template ToolTip
    /// </summary>
    public class SqlTemplate
    {

        public string TemplateText { get; }
        public string TemplateName { get; }
        public string ToolTip { get; }
        /// <summary>
        /// Constructor SQL Template
        /// </summary>
        /// <param name="templateName"></param>
        /// <param name="templateText"></param>
        /// <param name="toolTip"></param>
        public SqlTemplate(string templateName, string templateText, string toolTip)
        {
            TemplateName = templateName;
            TemplateText = templateText;
            ToolTip = toolTip;
        }
    }
}
