using CMS.Base;
using CMS.Localization;
using Controllers;
using Kentico.Caching.Example;
using KMVCHelper;
using Models.Examples;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;
using Kentico.Caching;
using CMS.SiteProvider;
using CMS.Helpers;

namespace Boilerplate.Controllers.Examples
{

    [KMVCRouteOverPathPriority]
    public class ExampleCacheController : Controller
    {
        public IExamplePageTypeRepository mExampleRepo;
        public readonly IOutputCacheDependencies mOutputCacheDependencies;

        public ExampleCacheController(IExamplePageTypeRepository ExampleRepo, IOutputCacheDependencies OutputCacheDependencies)
        {
            // Use constructor injection to get a handle on our ExampleService
            mExampleRepo = ExampleRepo;

            // Ability to add Kentico Cache Dependencies to OutputCache
            mOutputCacheDependencies = OutputCacheDependencies;
        }

        // GET: Examples
        public ActionResult Index()
        {
            // This call will be cached automaticall since it is a ".Get_____"
            ExamplePageTypeModel ExamplePage = mExampleRepo.GetExamplePages().FirstOrDefault();
            return View(ExamplePage);
        }

        public ActionResult IndexByID(int ID)
        {
            // This call will be cached automaticall since it is a ".Get_____"
            ExamplePageTypeModel ExamplePage = mExampleRepo.GetExamplePage(ID);
            return View("Index",ExamplePage);
        }

        /// <summary>
        /// See Global.asax.cs's GetVaryByCustomString.
        /// Cached View that will have a CacheName of  KenticoUser=Public|Example=HelloWorld (from Example's VaryByUser + VaryByExample)
        /// It will also clear the cache on the dependency of  "nodes|{SiteContext.CurrentSiteName}|{ExamplePageType.TYPEINFO}|all" and "CustomKey" to the HttpResponse Cache Dependencies
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 600, VaryByCustom = "Example")]
        public ActionResult CachedView()
        {
            // This call will be cached automaticall since it is a ".Get_____"
            ExamplePageTypeModel ExamplePage = mExampleRepo.GetExamplePages().FirstOrDefault();

            // Add proper Cache Dependencies
            mOutputCacheDependencies.AddCacheItemDependencies(mExampleRepo.GetExamplePagesCacheDependency());
            mOutputCacheDependencies.AddCacheItemDependency("CustomKey");
            return View(ExamplePage);
        }

        /// <summary>
        /// See Global.asax.cs's GetVaryByCustomString.
        /// Cached View that will have a CacheName of KenticoUser=Public|Example=HelloWorld|1|Blah for /ExampleCache/CachedView?ID=1&SomeString=Blah
        /// It will also clear the cache on the dependency of "nodeid|1" and "CustomKey" to the HttpResponse's Dependency for /ExampleCache/CachedViewByID?ID=1&SomeString=Blah
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 600, VaryByParam = "ID;SomeString", VaryByCustom = "Example")]
        public ActionResult CachedViewByID(int ID, string SomeString)
        {
            // This call will be cached automaticall since it is a ".Get_____"
            ExamplePageTypeModel ExamplePage = mExampleRepo.GetExamplePage(ID);

            // Add proper Cache Dependencies
            mOutputCacheDependencies.AddCacheItemDependencies(mExampleRepo.GetExamplePageCacheDependency(ID));
            mOutputCacheDependencies.AddCacheItemDependency("CustomKey");

            return View("CachedView", ExamplePage);
        }

        /// <summary>
        /// Will "touch" the Custom Key, Kentico handles touching Kentico cache dependencies
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult ClearCustomCache()
        {
            CacheHelper.TouchKey("CustomKey");
            return Json(new { Result = true }, JsonRequestBehavior.AllowGet);
        }
    }
}