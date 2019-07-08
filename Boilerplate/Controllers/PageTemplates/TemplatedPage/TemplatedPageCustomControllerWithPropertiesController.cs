using CMS.DocumentEngine;
using Kentico.PageBuilder.Web.Mvc.PageTemplates;
using KMVCHelper;
using Models.TemplatedPage;
using System.Web.Mvc;

namespace Controllers.PageTemplates
{
    public class TemplatedPageCustomControllerWithPropertiesController : PageTemplateController<TemplatedPageCustomProperties>
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

            TemplatedPageCustomProperties Props = GetProperties();
            // Generate your own view model.
            TemplatedPageCustomControllerWithPropertiesViewModel Model = new TemplatedPageCustomControllerWithPropertiesViewModel()
            {
                Message = "Hello World! Custom Message",
                ShowHelloWorld = Props.ShowHelloWorld,
                HelloWorldText = Props.HelloWorldText
            };
            
            // Have to hard code as the normal Return View() still has routing context of Home page controller
            return View("PageTemplates/_TemplatedPageCustomControllerWithProperties", Model);
        }
    }
}