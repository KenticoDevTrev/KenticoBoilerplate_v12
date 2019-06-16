using Kentico.PageBuilder.Web.Mvc;

namespace Models.Widgets
{
    /// <summary>
    /// Properties for Editable Text widget.
    /// </summary>
    public class EditableTextWidgetProperties : IWidgetProperties
    {
        /// <summary>
        /// HTML formatted text.
        /// </summary>
        public string Html { get; set; }
    }
}