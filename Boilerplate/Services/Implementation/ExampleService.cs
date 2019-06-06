using Boilerplate.Services.Interfaces;
using CMS.Base;
using CMS.Core;
using CMS.Localization;
using KMVCHelper;
using Models.Examples;
using System;
using System.Collections.Generic;

namespace Boilerplate.Services.Implementation
{
    /// <summary>
    /// This class is an example of how to use Dependency Injection to get common classes and services without having to do the hard work of intantiating them on your own.
    /// Some high level reasons that may prove useful: reusability (forces modular development) and testability (you can mock interfaces in order to properly test your written code)
    /// </summary>
    public class ExampleService : IExampleService
    {
        // Below are Kentico classes that are coded to an intercace and can be injected via Constructor Injection
        public ISiteService _siteService;
        public IEventLogService _eventLogService;
        public IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// Any parameters in the signature should be injected via constructor injection, if invoking this service direclty you will need to instantiate your own
        /// </summary>
        /// <param name="siteService"></param>
        /// <param name="eventLogService"></param>
        /// <param name="httpContextAccessor"></param>
        public ExampleService(ISiteService siteService, IEventLogService eventLogService, IHttpContextAccessor httpContextAccessor)
        {
            _siteService = siteService;
            _eventLogService = eventLogService;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Simply gets the current node using the absolute path of the HttpContext.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public ITreeNode GetCurrentNode()
        {
            var absolutePath = _httpContextAccessor.HttpContext.Request.Url.AbsolutePath;
            ITreeNode FoundNode = DocumentQueryHelper.GetNodeByAliasPath(absolutePath);
            return FoundNode;
        }

        /// <summary>
        /// Gets a list of `Demo.Banner` items under the `Node` passed in.
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public IEnumerable<ExampleBannersBanner> GetBannersFromNode(ITreeNode node)
        {
            var bannerList = new List<ExampleBannersBanner>();
            try
            {
                foreach (ITreeNode Banner in DocumentQueryHelper.RepeaterQuery(ClassNames: "Demo.Banner", RelationshipName: "Banners", RelationshipWithNodeGuid: node.NodeGUID))
                {
                    bannerList.Add(new ExampleBannersBanner()
                    {
                        BannerName = Banner.GetValue("BannerName").ToString(),
                        BannerUrl = Banner.GetValue("BannerImage").ToString()
                    });
                }
            }
            catch (Exception ex)
            {
                _eventLogService.LogException("ExampleService", "GET", ex);

            }
            return bannerList;
        }

        /// <summary>
        /// Gets a list of `SubNav` items under the `Node` passed in.
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public IEnumerable<SubNav> GetSubNavFromNode(ITreeNode node)
        {
            var subnavList = new List<SubNav>();
            try
            {
                foreach (ITreeNode Node in DocumentQueryHelper.RepeaterQuery(
                       Path: node.NodeAliasPath + "/%",
                       ClassNames: "CMS.MenuItem",
                       OrderBy: "NodeLevel, NodeOrder",
                       Columns: "MenuItemName,NodeAliasPath"
                       ))
                {
                    subnavList.Add(new SubNav()
                    {
                        LinkText = Node.GetValue("MenuItemName").ToString(),
                        // You have to decide what your URL will be, for us our URLs = NodeAliasPath
                        LinkUrl = Node.NodeAliasPath
                    });
                }
            }
            catch (Exception ex)
            {
                _eventLogService.LogException("ExampleService", "GET", ex);
            }
            return subnavList;
        }

        /// <summary>
        /// Gets a list of `SubNav` items under the `Node` passed in.
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public IEnumerable<SubNav> GetSubNavFromAliasPath(string nodeAliasPath, CultureInfo cultureInfo = null, ISiteInfo siteInfo = null)
        {
            if (siteInfo == null)
            {
                // If site is not provided, get the current site
                siteInfo = _siteService.CurrentSite;
                _eventLogService.LogEvent("GetSubNavFromAliasPath", "ExampleService", "GET", "Using current site");
            }
            else
            {
                _eventLogService.LogEvent("GetSubNavFromAliasPath", "ExampleService", "GET", "Using passed in site");
            }

            if(cultureInfo == null)
            {
                cultureInfo = LocalizationContext.GetCurrentCulture();
                _eventLogService.LogEvent("GetSubNavFromAliasPath", "ExampleService", "GET", "Using current culture");
            }
            else
            {
                _eventLogService.LogEvent("GetSubNavFromAliasPath", "ExampleService", "GET", "Using passed in culture");
            }

            var subnavList = new List<SubNav>();
            try
            {
                foreach (ITreeNode Node in DocumentQueryHelper.RepeaterQuery(
                Path: nodeAliasPath + "/%",
                CultureCode: cultureInfo.CultureCode,
                SiteName: siteInfo.SiteName,
                ClassNames: "CMS.MenuItem",
                OrderBy: "NodeLevel, NodeOrder",
                Columns: "MenuItemName,NodeAliasPath"
                ))
                {
                    subnavList.Add(new SubNav()
                    {
                        LinkText = Node.GetValue("MenuItemName").ToString(),
                        // You have to decide what your URL will be, for us our URLs = NodeAliasPath
                        LinkUrl = Node.NodeAliasPath
                    });
                }
            }
            catch (Exception ex)
            {
                _eventLogService.LogException("ExampleService", "GET", ex);
            }
            return subnavList;
        }
    }
}