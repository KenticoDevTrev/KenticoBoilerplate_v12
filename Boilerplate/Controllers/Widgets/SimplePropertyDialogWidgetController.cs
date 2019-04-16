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

[assembly: RegisterWidget("Widgets.SimplePropertyDialogWidget", typeof(SimplePropertyDialogWidgetController), "Display the Number", Description = "A very simple widget with Dialog.", IconClass = "icon-one")]
namespace Controllers.Widgets
{
    public class SimplePropertyDialogWidgetController : WidgetController<SimplePropertyDialogWidgetProperties>
    {
        public SimplePropertyDialogWidgetController()
        {

        }


        /// <summary>
        /// Creates an instance of <see cref="SimplePropertyDialogWidgetController"/> class.
        /// </summary>
        /// <param name="propertiesRetriever">Retriever for widget properties.</param>
        /// <param name="currentPageRetriever">Retriever for current page where is the widget used.</param>
        /// <remarks>Use this constructor for tests to handle dependencies.</remarks>
        public SimplePropertyDialogWidgetController(IWidgetPropertiesRetriever<SimplePropertyDialogWidgetProperties> propertiesRetriever,
                                        ICurrentPageRetriever currentPageRetriever) : base(propertiesRetriever, currentPageRetriever)
        {
        }

        // GET: TextWidget
        public ActionResult Index()
        {
            var currentPage = GetPage();
            var properties = GetProperties();
            return PartialView("Widgets/SimplePropertyDialogWidget", new SimplePropertyDialogWidgetViewModel { Number = properties.Number });
        }
    }
}