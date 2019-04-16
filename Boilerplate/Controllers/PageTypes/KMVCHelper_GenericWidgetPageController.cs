using KMVCHelper;
using KMVCHelper.Models;
using System.Web.Mvc;

namespace Controllers
{
    public class KMVCHelper_GenericWidgetPageController : BaseController
    {
        // GET: GenericWidgetPage
        public ActionResult Index()
        {
            GenericWidgetPage FoundNode = (GenericWidgetPage)DocumentQueryHelper.GetNodeByAliasPath(HttpContext.Request.Url.AbsolutePath, GenericWidgetPage.OBJECT_TYPE);
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