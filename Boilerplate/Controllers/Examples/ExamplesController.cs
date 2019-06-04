using Boilerplate.Services.Interfaces;
using CMS.Base;
using CMS.DocumentEngine;
using CMS.Localization;
using CMS.SiteProvider;
using Controllers;
using KMVCHelper;
using Models.Examples;
using RelationshipsExtended;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Boilerplate.Controllers.Examples
{

    [KMVCRouteOverPathPriority]
    public class ExamplesController : BaseController
    {
        private IExampleService _exampleService;
        public ExamplesController(IExampleService exampleService)
        {
            // Use constructor injection to get a handle on our ExampleService
            _exampleService = exampleService;
        }

        // GET: Examples
        public ActionResult Index()
        {
            return View();
        }

        #region "Example View"

        public ActionResult Banners()
        {
            ITreeNode FoundNode = _exampleService.GetCurrentNode();
            if (FoundNode != null)
            {
                SetContext(FoundNode.DocumentID);
                // Get Banners
                ExampleBannersViewModel Model = new ExampleBannersViewModel();
                Model.BannerNameUrlsList = _exampleService.GetBannersFromNode(FoundNode);
                return View(Model);
            }
            else
            {
                return HttpNotFound("Could not find page by that Url");
            }
        }

        #endregion  

        #region "Web Part Equivelant in MVC"

        /// <summary>
        /// Example of how to do Web Parts in MVC using RenderAction, and in this case no model is provided for the webpart, just using
        /// the ViewBag Context set by SetContext, similar to in Portal Method how you would use Macros.
        /// </summary>
        /// <returns></returns>
        public ActionResult MVCWebPartsNoModel()
        {
            ITreeNode FoundNode = _exampleService.GetCurrentNode();
            if (FoundNode != null)
            {
                // This will give us the Node Alias Path context
                SetContext(FoundNode.DocumentID);
                return View();
            }
            else
            {
                return HttpNotFound("Could not find page by that Url");
            }
        }

        /// <summary>
        /// Example of how to do Web Parts in MVC Using RenderAction, and in this case the parent Controller is filling a model
        /// that will be passed to the "Web Part" (Action) in order to render.  More leg work but also source-agnostic. (Model-View-ViewModel MVVM)
        /// </summary>
        /// <returns></returns>
        public ActionResult MVCWebPartsViewModel()
        {
            ITreeNode FoundNode = _exampleService.GetCurrentNode();
            if (FoundNode != null)
            {
                ExampleMVCWebPartsViewModel Model = new ExampleMVCWebPartsViewModel();
                // Get subnav items from the ExampleService
                Model.SubNavigation = _exampleService.GetSubNavFromNode(FoundNode);
                return View(Model);
            }
            else
            {
                return HttpNotFound("Could not find page by that Url");
            }
        }

        /// <summary>
        /// This Repeater "Web Part" is given the ViewBag (since it's normally not available on child actions) and configures itself from this.
        /// This allows for easy calling and adjustment to this "Web Parts" Logic without really needing adjustments on the calling Views, but
        /// is very much tied into the source (ex: Kentico).  It would be hard to flip this with Kentico Cloud if you wished to use that.
        /// </summary>
        /// <param name="ViewBag">The ViewBag which will have the Document, Culture, and Site Context</param>
        /// <returns></returns>
        public ActionResult NavigationByContext(dynamic ViewBag)
        {
            // Build the actual Partial View's model from the data provided by the parent View
            ExampleMVCWebPartsSubNavs Model = new ExampleMVCWebPartsSubNavs();
            var subNavs = new List<SubNav>();
            // Get the Sub Nav Items
            foreach (TreeNode Node in DocumentQueryHelper.RepeaterQuery(
                Path: ViewBag.CurrentDocument.NodeAliasPath + "/%",
                CultureCode: ((CultureInfo)ViewBag.CurrentCulture).CultureCode,
                SiteName: ((SiteInfo)ViewBag.CurrentSite).SiteName,
                ClassNames: "CMS.MenuItem",
                OrderBy: "NodeLevel, NodeOrder",
                Columns: "MenuItemName,NodeAliasPath"
                ))
            {
                subNavs.Add(new SubNav()
                {
                    LinkText = Node.GetValue("MenuItemName", ""),
                    // You have to decide what your URL will be, for us our URLs = NodeAliasPath
                    LinkUrl = Node.NodeAliasPath
                });
            }

            Model.SubNavigation = subNavs;
            return View("Navigation", Model);
        }

        /// <summary>
        /// This Repeater "Webpart" Relies on just a path that would be provided through the View's context.  Does not rely on passing
        /// the ViewBag like the NavigationByContext, but does then require the calling View to provide the properties, and if ever more
        /// properties are needed, would need to adjust both Controller and View alike.
        /// </summary>
        /// <returns></returns>
        public ActionResult NavigationByPath(string Path, string Culture, string SiteName)
        {
            // Build the actual Partial View's model from the data provided by the parent View
            ExampleMVCWebPartsSubNavs Model = new ExampleMVCWebPartsSubNavs();
            // Get the Sub Nav Items from the ExampleService
            Model.SubNavigation = _exampleService.GetSubNavFromAliasPath(Path, CultureInfoProvider.GetCultureInfo(Culture), SiteInfoProvider.GetSiteInfo(SiteName));
            return View("Navigation", Model);
        }

        /// <summary>
        /// This Repeater "Web Part" relies completely on being passed the proper Model which is platform agnostic, so you could
        /// provide a List of SubNavs generated from Kentico, Kentico Cloud, or some other source.
        /// </summary>
        /// <param name="SubNavs">The Sub Nav List (Passed from the View's Model)</param>
        /// <returns></returns>
        public ActionResult NavigationByModel(IEnumerable<SubNav> SubNavs)
        {
            // Build the actual Partial View's model from the data provided by the parent View
            ExampleMVCWebPartsSubNavs Model = new ExampleMVCWebPartsSubNavs()
            {
                SubNavigation = SubNavs
            };
            return View("Navigation", Model);
        }

        #endregion

    }
}