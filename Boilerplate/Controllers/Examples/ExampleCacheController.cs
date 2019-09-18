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
using System;
using System.Web.Caching;
using System.Web;

namespace Boilerplate.Controllers.Examples
{

    [KMVCRouteOverPathPriority]
    public class ExampleCacheController : Controller
    {
        public IExamplePageTypeRepository mExamplePageTypeRepo;
        public IExampleModuleClassRepository mExampleModuleClassRepo;
        public readonly IOutputCacheDependencies mOutputCacheDependencies;

        public ExampleCacheController(IExamplePageTypeRepository ExampleRepo, IExampleModuleClassRepository ExampleModuleClassRepo, IOutputCacheDependencies OutputCacheDependencies)
        {
            // Use constructor injection to get a handle on our ExampleService
            mExamplePageTypeRepo = ExampleRepo;
            mExampleModuleClassRepo = ExampleModuleClassRepo;

            // Ability to add Kentico Cache Dependencies to OutputCache
            mOutputCacheDependencies = OutputCacheDependencies;
        }

        // GET: Examples
        public ActionResult Index()
        {
            // This call will be cached automaticall since it is a ".Get_____"
            ExamplePageTypeModel ExamplePage = mExamplePageTypeRepo.GetExamplePages().FirstOrDefault();
            return View(ExamplePage);
        }

        /// <summary>
        /// This action, since it is not OutputCache'd, will always execute, however the mExampleRepo.GetExamplePage(ID) will return a cached result based on the ID, until that node is updated
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public ActionResult IndexByID(int ID)
        {
            // This call will be cached automaticall since it is a ".Get_____"
            ExamplePageTypeModel ExamplePage = mExamplePageTypeRepo.GetExamplePage(ID);
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
            ExamplePageTypeModel ExamplePage = mExamplePageTypeRepo.GetExamplePages().FirstOrDefault();

            // Add proper Cache Dependencies
            mOutputCacheDependencies.AddCacheItemDependencies(mExamplePageTypeRepo.GetExamplePagesCacheDependency());
            mOutputCacheDependencies.AddCacheItemDependency("CustomKey");
            return View(ExamplePage);
        }

        /// <summary>
        /// See Global.asax.cs's GetVaryByCustomString.
        /// Cached View that will have a theoretical CacheName of KenticoUser=Public|Example=HelloWorld|1|Blah for /ExampleCache/CachedViewByID?ID=1&SomeString=Blah
        /// It will also clear the cache on the dependency of "nodeid|1" and "CustomKey" to the HttpResponse's Dependency for /ExampleCache/CachedViewByID?ID=1&SomeString=Blah
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 600, VaryByParam = "ID;SomeString", VaryByCustom = "Example")]
        //[ActionResultCache(Duration = 600, VaryByParam = "ID;SomeString", VaryByCustom = "Example")]
        public ActionResult CachedViewByID(int ID, string SomeString)
        {
            // This call will be cached automaticall since it is a ".Get_____"
            ExamplePageTypeModel ExamplePage = mExamplePageTypeRepo.GetExamplePage(ID);

            // Add proper Cache Dependencies
            mOutputCacheDependencies.AddCacheItemDependencies(mExamplePageTypeRepo.GetExamplePageCacheDependency(ID));
            mOutputCacheDependencies.AddCacheItemDependency("CustomKey");
            return View("CachedView", ExamplePage);
        }

        /// <summary>
        /// See Global.asax.cs's GetVaryByCustomString.
        /// Caches the ActionResult, but not the View Rendering.  This is useful if you want to Cache the Logic, but leave the View uncached so you can implement more Donut-hole typed caching (cache individual components instead of the whole)
        /// Cached View that will have a theoretical CacheName of KenticoUser=Public|Example=HelloWorld|1|Blah for /ExampleCache/CachedActionByID?ID=1&SomeString=Blah
        /// It will also clear the cache on the dependency of "nodeid|1" and "CustomKey" to the HttpResponse's Dependency for /ExampleCache/CachedActionByID?ID=1&SomeString=Blah
        /// </summary>
        /// <returns></returns>
        [ActionResultCache(Duration = 600, VaryByParam = "ID;SomeString", VaryByCustom = "Example")]
        public ActionResult CachedActionByID(int ID, string SomeString)
        {
            // This call will be cached automaticall since it is a ".Get_____"
            ExamplePageTypeModel ExamplePage = mExamplePageTypeRepo.GetExamplePage(ID);

            // Add proper Cache Dependencies
            mOutputCacheDependencies.AddCacheItemDependencies(mExamplePageTypeRepo.GetExamplePageCacheDependency(ID));
            mOutputCacheDependencies.AddCacheItemDependency("CustomKey");
            return View("CachedView", ExamplePage);
        }

        /// <summary>
        /// Will "touch" the CustomKey, Kentico handles touching Kentico cache dependencies
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult ClearCustomCache()
        {
            CacheHelper.TouchKey("CustomKey");
            return Json(new { Result = true }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Caches this by the ItemNum
        /// </summary>
        /// <param name="Index">The index of the ExampleModule you wish to get.</param>
        /// <returns></returns>
        [OutputCache(Duration =600, VaryByParam ="*")]
        public ActionResult CachedPartialExample(int Index)
        {
            var ExampleClassItems = mExampleModuleClassRepo.GetExampleModuleClasses();

            // Add cache dependency
            mOutputCacheDependencies.AddCacheItemDependencies(mExampleModuleClassRepo.GetExampleModuleClassesCacheDependency());

            if(ExampleClassItems.Count() >= Index)
            {
                return View("CachedPartialExample", ExampleClassItems.ToList()[Index-1]);
            } else
            {
                return Content("");
            }
        }

    }
}