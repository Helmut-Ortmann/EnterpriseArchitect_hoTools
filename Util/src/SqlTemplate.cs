using System;
using System.Resources;
using System.Reflection;


// ReSharper disable once CheckNamespace
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

        public string TemplateText {
            get
            {
                if (IsResource)
                {
                    ResourceManager rm = new ResourceManager("hoTools.Utils.Resources.Strings", Assembly.GetExecutingAssembly());
                    return rm.GetString(_templateText);
                }
                return _templateText;
            }
           
        }
        string _templateText;
        public string TemplateName { get; }
        public string ToolTip { get; }
        public Boolean IsResource { get; }
        /// <summary>
        /// Constructor SQL Template with the resource as Text
        /// </summary>
        /// <param name="templateName"></param>
        /// <param name="templateText"></param>
        /// <param name="toolTip"></param>
        public SqlTemplate(string templateName, string templateText, string toolTip)
        {
            TemplateName = templateName;
            _templateText = templateText;
            ToolTip = toolTip;
            IsResource = false;
        }

        /// <summary>
        /// Constructor SQL Template with the resource as Resource name
        /// </summary>
        /// <param name="templateName"></param>
        /// <param name="templateText"></param>
        /// <param name="toolTip"></param>
        /// <param name="isResource"></param>
        public SqlTemplate(string templateName, string templateText, string toolTip, bool isResource)
        {
            TemplateName = templateName;
            _templateText = templateText;
            ToolTip = toolTip;
            IsResource = isResource;
        }
    }
}
