using Boilerplate.Controllers.PageTemplates;
using CMS.DocumentEngine;
using Kentico.PageBuilder.Web.Mvc.PageTemplates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

[assembly: RegisterPageTemplate("Home.Template3", typeof(HomePageTemplate3Controller), "Template 3")]
namespace Boilerplate.Controllers.PageTemplates
{
    public class HomePageTemplate3Controller : PageTemplateController
    {
        // GET: HomePageTemplates
        public ActionResult Index()
        {

            // Retrieves the page from the Kentico database
            TreeNode page = DocumentHelper.GetDocuments()
                .Path("/Home")
                .OnCurrentSite()
                .TopN(1)
                .FirstOrDefault();

            // Returns a 404 error when the retrieving is unsuccessful
            if (page == null)
            {
                return HttpNotFound();
            }

            // Have to hard code as the normal Return View() still has routing context of Home page controller
            return View("HomePageTemplate3/Index");
        }
    }
}