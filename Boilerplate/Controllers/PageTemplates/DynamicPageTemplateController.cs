using CMS.DocumentEngine;
using Kentico.PageBuilder.Web.Mvc;
using Kentico.PageBuilder.Web.Mvc.PageTemplates;
using Kentico.Web.Mvc;
using KMVCHelper;
using System.Web.Mvc;

namespace Controllers.PageTemplates
{
    /// <summary>
    /// This class handles Page Templates for pages that are using the Dynamic Routing
    /// </summary>
    public class DynamicPageTemplateController : PageTemplateController
    {
        // GET: DynamicPageTemplate, finds the node based on the current request url and then renders the template result
        public ActionResult Index()
        {
            TreeNode FoundNode = DocumentQueryHelper.GetNodeByAliasPath(EnvironmentHelper.GetUrl(HttpContext.Request));
            if (FoundNode != null)
            {
                HttpContext.Kentico().PageBuilder().Initialize(FoundNode.DocumentID);
                return new TemplateResult(FoundNode.DocumentID);
            } else
            {
                return new HttpNotFoundResult();
            }
        }

        public ActionResult NotFound()
        {
            return new HttpNotFoundResult("No template selected for this page.");
        }

        public ActionResult UnregisteredTemplate()
        {
            TreeNode FoundNode = DocumentQueryHelper.GetNodeByAliasPath(EnvironmentHelper.GetUrl(HttpContext.Request));
            if (FoundNode != null)
            {
                HttpContext.Kentico().PageBuilder().Initialize(FoundNode.DocumentID);
                return View();
            }
            else
            {
                return new HttpNotFoundResult();
            }
            
        }
    }
}