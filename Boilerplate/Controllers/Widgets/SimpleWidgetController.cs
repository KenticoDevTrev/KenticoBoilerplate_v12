using CMS.DocumentEngine;
using CMS.EventLog;
using Controllers.Widgets;
using Kentico.PageBuilder.Web.Mvc;
using Models.Widgets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

[assembly: RegisterWidget("Widgets.SimpleWidget", typeof(SimpleWidgetController), "Display the Text", Description = "A very simple widget.", IconClass = "icon-one")]
namespace Controllers.Widgets
{
    public class SimpleWidgetController : WidgetController<SimpleWidgetProperties>
    {
        public SimpleWidgetController()
        {

        }


        /// <summary>
        /// Creates an instance of <see cref="SimpleWidgetController"/> class.
        /// </summary>
        /// <param name="propertiesRetriever">Retriever for widget properties.</param>
        /// <param name="currentPageRetriever">Retriever for current page where is the widget used.</param>
        /// <remarks>Use this constructor for tests to handle dependencies.</remarks>
        public SimpleWidgetController(IWidgetPropertiesRetriever<SimpleWidgetProperties> propertiesRetriever,
                                        ICurrentPageRetriever currentPageRetriever) : base(propertiesRetriever, currentPageRetriever)
        {
        }

        // GET: TextWidget
        public ActionResult Index()
        {
            var currentPage = GetPage();
            var properties = GetProperties();
            return PartialView("Widgets/SimpleWidget", new SimpleWidgetViewModel { Text = properties.Text });
        }
    }
}