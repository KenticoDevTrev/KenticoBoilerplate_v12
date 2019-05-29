using System.Web.Mvc;
using Models.Widgets;
using Controllers.Widgets;
using Kentico.PageBuilder.Web.Mvc;

[assembly: RegisterWidget("KMVC.EditableText", typeof(EditableTextWidgetController), "Editable Text", Description = "Inline WYSIWYG Editor", IconClass = "icon-w-editable-text")]

namespace Controllers.Widgets
{
    public class EditableTextWidgetController : WidgetController<EditableTextWidgetProperties>
    {
        /// <summary>
        /// Creates an instance of <see cref="EditableTextWidgetController"/> class.
        /// </summary>
        public EditableTextWidgetController()
        {
        }


        /// <summary>
        /// Creates an instance of <see cref="EditableTextWidgetController"/> class.
        /// </summary>
        /// <param name="propertiesRetriever">Retriever for widget properties.</param>
        /// <param name="currentPageRetriever">Retriever for current page where is the widget used.</param>
        /// <remarks>Use this constructor for tests to handle dependencies.</remarks>
        public EditableTextWidgetController(IWidgetPropertiesRetriever<EditableTextWidgetProperties> propertiesRetriever,
                                        ICurrentPageRetriever currentPageRetriever) : base(propertiesRetriever, currentPageRetriever)
        {
        }


        // GET: TextWidget
        public ActionResult Index()
        {
            var properties = GetProperties();
            return PartialView("Widgets/_EditableTextWidget", new EditableTextWidgetViewModel { Html = properties.Html }); 
        }
    }
}