using CMS.DataEngine;
using CMS.DocumentEngine;
using Kentico.PageBuilder.Web.Mvc;
using Kentico.Web.Mvc;
using KMVCHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Controllers
{
    [KMVCRouteOverPathPriority]
    public class HomeController : BaseController
    {
        // GET: Home
        public ActionResult Index()
        {
            // Get the current page, in this case we know it's the Home page
            var HomePage = DocumentQueryHelper.RepeaterQuery(Path: "/Home", ClassNames: "MVC.Home", CacheItemName: "HomePage").GetTypedResult().Items.FirstOrDefault();
            HttpContext.Kentico().PageBuilder().Initialize(HomePage.DocumentID);
            SetContext(HomePage.DocumentID);
            return View();
        }
    }

}