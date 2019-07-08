using CMS.DocumentEngine;
using Kentico.PageBuilder.Web.Mvc.PageTemplates;
using KMVCHelper;
using Models.TemplatedPage;
using System.Web.Mvc;

namespace Controllers.PageTemplates
{
    public class TemplatedPageCustomControllerController : PageTemplateController
    {

        // GET: HomePageTemplates
        public ActionResult Index()
        {
            // Retrieves the page from the Kentico database
            TreeNode page = DocumentQueryHelper.GetNodeByAliasPath(EnvironmentHelper.GetUrl(HttpContext.Request));

            // Returns a 404 error when the retrieving is unsuccessful
            if (page == null)
            {
                return HttpNotFound();
            }

            // Generate your own view model.
            TemplatedPageCustomControllerViewModel Model = new TemplatedPageCustomControllerViewModel()
            {
                Message = "Hello World! Custom Message"
            };
            
            // Have to hard code as the normal Return View() still has routing context of Home page controller
            return View("PageTemplates/_TemplatedPageCustomController", Model);
        }
    }


}