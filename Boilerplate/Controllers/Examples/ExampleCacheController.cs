using CMS.Base;
using CMS.Localization;
using Controllers;
using Kentico.Caching.Example;
using KMVCHelper;
using Models.Examples;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;

namespace Boilerplate.Controllers.Examples
{

    [KMVCRouteOverPathPriority]
    public class ExampleCacheController : BaseController
    {
        public IExamplePageTypeRepository _exampleRepo;
        public ExampleCacheController(IExamplePageTypeRepository exampleRepo)
        {
            // Use constructor injection to get a handle on our ExampleService
            _exampleRepo = exampleRepo;
        }

        // GET: Examples
        public ActionResult Index()
        {
            ExamplePageTypeModel ExamplePage = _exampleRepo.GetExamplePages().FirstOrDefault();
            return View(ExamplePage);
        }

    }
}