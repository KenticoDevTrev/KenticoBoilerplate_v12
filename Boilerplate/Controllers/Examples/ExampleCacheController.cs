using MVCCaching.Kentico.Example;
using KMVCHelper;
using System.Web.Mvc;
using System.Linq;
using MVCCaching.Kentico;
using System.Web.Caching;
using System.Collections;
using System.Collections.Generic;

namespace Boilerplate.Controllers.Examples
{
    [KMVCRouteOverPathPriority]
    public class ExampleCacheController : Controller
    {
        private string ExampleType = "GenericModel";

        private IExamplePageTypeRepository mExamplePageTypeRepo;
        private IExampleModuleClassRepository mExampleModuleClassRepo;
        private readonly IOutputCacheDependencies mOutputCacheDependencies;
        private readonly ICacheHelper mCacheHelper;

        public ExampleCacheController(IExamplePageTypeRepository ExampleRepo, IExampleModuleClassRepository ExampleModuleClassRepo, IOutputCacheDependencies OutputCacheDependencies, ICacheHelper CacheHelper)
        {
            // Use constructor injection to get a handle on our ExampleService
            mExamplePageTypeRepo = ExampleRepo;
            mExampleModuleClassRepo = ExampleModuleClassRepo;

            // Ability to add Kentico Cache Dependencies to OutputCache
            mOutputCacheDependencies = OutputCacheDependencies;
            mCacheHelper = CacheHelper;
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
            return View("Index", ExamplePage);
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
            mCacheHelper.TouchKey("CustomKey");
            return Json(new { Result = true }, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// Caches this by the ItemNum
        /// </summary>
        /// <param name="Index">The index of the ExampleModule you wish to get.</param>
        /// <returns></returns>
        [OutputCache(Duration = 600, VaryByParam = "*")]
        public ActionResult CachedPartialExample(int Index)
        {
            // Get the Items
            var Items = mExampleModuleClassRepo.GetExampleModuleClasses();

            // Add cache dependency
            mOutputCacheDependencies.AddCacheItemDependencies(mExampleModuleClassRepo.GetExampleModuleClassesCacheDependency());

            // Convert to View Model
            if (Items.Count() >= Index)
            {
                var Model = new ExampleModuleClassViewModel()
                {
                    Name = Items.ToList()[Index - 1].Name
                };
                return View("CachedPartialExample", Model);
            }
            else
            {
                return Content("");
            }
        }

        /// <summary>
        /// Caches this by the ItemNum, this uses the BaseInfo returning method to show the difference
        /// </summary>
        /// <param name="Index">The index of the ExampleModule you wish to get.</param>
        /// <returns></returns>
        [OutputCache(Duration = 600, VaryByParam = "*")]
        public ActionResult CachedPartialExample_BaseInfo(int Index)
        {
            // Get the BaseInfo models
            var Items = mExampleModuleClassRepo.GetExampleModuleClasses_BaseInfo();

            // Add cache dependency
            mOutputCacheDependencies.AddDependencyOnInfoObjects<ExampleModuleClassInfo>();

            if (Items.Count() >= Index)
            {
                // Convert to View Model
                var Model = new ExampleModuleClassViewModel()
                {
                    Name = Items.First().ExampleModuleClassName
                };

                return View("CachedPartialExample", Model);
            }
            else
            {
                return Content("");
            }
        }

        /// <summary>
        /// Caches this by the ID
        /// </summary>
        /// <param name="Index">The index of the ExampleModule you wish to get.</param>
        /// <returns></returns>
        [OutputCache(Duration = 600, VaryByParam = "*")]
        public ActionResult CachedPartialExampleByID(int ID)
        {
            // Get the Item
            var Item = mExampleModuleClassRepo.GetExampleModuleClass(ID);

            // Add cache dependency
            mOutputCacheDependencies.AddCacheItemDependencies(mExampleModuleClassRepo.GetExampleModuleClassCacheDependency(ID));

            // Convert to View Model
            var Model = new ExampleModuleClassViewModel()
            {
                Name = Item.Name
            };

            // Return the item
            return View("CachedPartialExample", Model);
        }

        /// <summary>
        /// Caches this by the ID, this uses the BaseInfo returning method to show the difference
        /// </summary>
        /// <param name="Index">The index of the ExampleModule you wish to get.</param>
        /// <returns></returns>
        [OutputCache(Duration = 600, VaryByParam = "*")]
        public ActionResult CachedPartialExampleByID_BaseInfo(int ID)
        {
            // Get the BaseInfo models
            var Item = mExampleModuleClassRepo.GetExampleModuleClass_BaseInfo(ID);

            // Add cache dependency
            mOutputCacheDependencies.AddDependencyOnInfoObject<ExampleModuleClassInfo>(Item.ExampleModuleClassGuid);

            // Convert to View Model
            var Model = new ExampleModuleClassModel()
            {
                Name = Item.ExampleModuleClassName
            };

            return View("CachedPartialExample", Model);

        }
    }
}