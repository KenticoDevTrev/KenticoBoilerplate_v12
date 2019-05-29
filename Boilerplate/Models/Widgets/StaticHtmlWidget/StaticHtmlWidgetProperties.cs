using Kentico.Forms.Web.Mvc;
using Kentico.PageBuilder.Web.Mvc;

namespace Models.Widgets
{
    /// <summary>
    /// Properties for Editable Text widget.
    /// </summary>
    public class StaticHtmlWidgetProperties : IWidgetProperties
    {
        [EditingComponent(Kentico.FormComponents.TinyMCEInputComponent.IDENTIFIER, Label = "Html Content")]

        /// <summary>
        /// HTML formatted text.
        /// </summary>
        public string Html { get; set; }
    }
}