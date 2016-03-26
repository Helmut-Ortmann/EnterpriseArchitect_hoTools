using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace hoTools.Utils.SQL
{
    public class SqlTemplate
    {

        public string Template { get; }
        public string TemplateName { get; }
        public string ToolTip { get; }
        public SqlTemplate(string templateName, string template, string toolTip)
        {
            TemplateName = templateName;
            Template = template;
            ToolTip = ToolTip;
        }
    }
}
