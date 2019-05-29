using System.Web.Mvc;
using Models.Widgets;
using Controllers.Widgets;
using Kentico.PageBuilder.Web.Mvc;

[assembly: RegisterWidget("KMVC.StaticHtml", typeof(StaticHtmlWidgetController), "Static Html", Description = "Configure and add html using a WYSIWYG editor", IconClass = "icon-w-static-html")]

namespace Controllers.Widgets
{
    public class StaticHtmlWidgetController : WidgetController<StaticHtmlWidgetProperties>
    {
        /// <summary>
        /// Creates an instance of <see cref="StaticHtmlWidgetController"/> class.
        /// </summary>
        public StaticHtmlWidgetController()
        {
        }


        /// <summary>
        /// Creates an instance of <see cref="StaticHtmlWidgetController"/> class.
        /// </summary>
        /// <param name="propertiesRetriever">Retriever for widget properties.</param>
        /// <param name="currentPageRetriever">Retriever for current page where is the widget used.</param>
        /// <remarks>Use this constructor for tests to handle dependencies.</remarks>
        public StaticHtmlWidgetController(IWidgetPropertiesRetriever<StaticHtmlWidgetProperties> propertiesRetriever,
                                        ICurrentPageRetriever currentPageRetriever) : base(propertiesRetriever, currentPageRetriever)
        {
        }


        // GET: TextWidget
        public ActionResult Index()
        {
            var properties = GetProperties();
            return PartialView("Widgets/_StaticHtmlWidget", new StaticHtmlWidgetViewModel { Html = properties.Html }); 
        }
    }
}