using Kentico.PageBuilder.Web.Mvc.PageTemplates;
using KMVCHelper;
using KMVCHelper.Models;
using System.Web.Mvc;

namespace Controllers
{
    public class KMVCHelper_GenericWidgetPageController : TemplatedBaseController
    {
        // GET: GenericWidgetPage
        public ActionResult Index()
        {
            GenericWidgetPage FoundNode = (GenericWidgetPage)DocumentQueryHelper.GetNodeByAliasPath(EnvironmentHelper.GetUrl(HttpContext.Request), GenericWidgetPage.OBJECT_TYPE);
            if (FoundNode != null)
            {
                SetContext(FoundNode.DocumentID);
                return View(FoundNode.Layout);
            }
            else
            {
                return HttpNotFound("Could not find page by that Url");
            }
        }
    }
}