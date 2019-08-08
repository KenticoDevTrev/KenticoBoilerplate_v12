using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Boilerplate.Controllers;
using CMS.DocumentEngine;
using CMS.Relationships;
using Kentico.PageBuilder.Web.Mvc;
using Kentico.PageBuilder.Web.Mvc.PageTemplates;
using Kentico.Web.Mvc;
using KMVCHelper;

namespace Controllers
{
    [KMVCRouteOverPathPriority]
    public class HomeController : PageTemplateController
    {
        // GET: Home
        public ActionResult Index()
        {
            // Uncomment and optionally adjust the document query sample when using Page builder on the Home page
            // See ~/App_Start/ApplicationConfig.cs, ~/Views/Shared/_Layout.cshtml and ~/Views/Home/Index.cshtml
            // In the administration UI, create a Page type and set its
            //   URL pattern = '/Home'
            //   Use Page tab = True
            // In the administration UI, create a Page utilizing the new Page type

            // Testing

            TreeNode page = DocumentHelper.GetDocuments().Path("/Home").OnCurrentSite().TopN(1).FirstOrDefault();
            if (page == null)
            {
                return HttpNotFound();
            }
            var Query = new MultiDocumentQuery().Path("/%").InRelationWith(page.NodeGUID, "MVC.Home_1616edf9-3837-498a-a94b-e8297355a37f", RelationshipSideEnum.Left);
            string QueryText = Query.ToString();
            RelationshipInfoProvider.ApplyRelationshipOrderData(Query, page.NodeID, RelationshipNameInfoProvider.GetRelationshipNameInfo("MVC.Home_1616edf9-3837-498a-a94b-e8297355a37f").RelationshipNameId);
            QueryText = Query.ToString();
            var Results = Query.TypedResult;


            HttpContext.Kentico().PageBuilder().Initialize(page.DocumentID);

            // Use template if it has one.
            if (KMVCDynamicHttpHandler.PageHasTemplate(page))
            {
                return new TemplateResult(page.DocumentID);
            }
            else
            {
                return View();
            }
        }
    }
}