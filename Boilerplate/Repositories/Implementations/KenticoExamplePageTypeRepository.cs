﻿using CMS.SiteProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kentico.Caching.Example
{

    public class KenticoExamplePageTypeRepository : IExamplePageTypeRepository
    {
        private readonly string mCultureName;
        private readonly bool mLatestVersionEnabled;
        private ICacheHelper mCacheHelper;

        public KenticoExamplePageTypeRepository(string cultureName, bool latestVersionEnabled, ICacheHelper CacheHelper)
        {
            mCultureName = cultureName;
            mLatestVersionEnabled = latestVersionEnabled;
            mCacheHelper = CacheHelper;
        }

        /// <summary>
        /// Gets the specific Example Page.  Adding a specific cache dependency
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [CacheDependency("nodeid|{0}")]
        public ExamplePageTypeModel GetExamplePage(int ID)
        {
            // Get the Page
            var Page = ExamplePageTypeProvider.GetExamplePageType(ID, mCultureName, SiteContext.CurrentSiteName)
                .LatestVersion(mLatestVersionEnabled)
                .Published(!mLatestVersionEnabled)
                .CombineWithDefaultCulture()
                .FirstOrDefault();

            // Convert to Model
            return new ExamplePageTypeModel()
            {
                Name = Page.Name,
                ID = Page.NodeID
            };
        }

        /// <summary>
        /// Gets the Cache Dependency for the given Example Page with the NodeID
        /// </summary>
        /// <param name="ID">The ID of the node</param>
        /// <returns>The Cache Dependency Key</returns>
        public IEnumerable<string> GetExamplePageCacheDependency(int ID)
        {
            return new string[] { $"nodeid|{ID}" };
        }

        /// <summary>
        /// Gets All the example pages, In this case the CacheDependency is dynamic with the Site Name, so we can't declare it on a Cache Dependency attribute, and must cache separately
        /// </summary>
        /// <returns>All the Example Pages</returns>        
        public IEnumerable<ExamplePageTypeModel> GetExamplePages()
        {
            // Get the Pages
            return mCacheHelper.Cache<IEnumerable<ExamplePageTypeModel>>(() =>
            {
                var Pages = ExamplePageTypeProvider.GetExamplePageTypes()
                .LatestVersion(mLatestVersionEnabled)
                .Published(!mLatestVersionEnabled)
                .OnSite(SiteContext.CurrentSiteName)
                .Culture(mCultureName)
                .CombineWithDefaultCulture()
                .OrderBy("NodeOrder")
                .ToList();

                // Convert to Model
                return Pages.Select(x =>
                {
                    return new ExamplePageTypeModel()
                    {
                        Name = x.Name,
                        ID = x.NodeID
                    };
                }).ToList();
            }, "KenticoExamplePageTypeRepository.GetExamplePages", GetExamplePagesCacheDependency(), mCacheHelper.CacheDuration());

            
        }

        public IEnumerable<string> GetExamplePagesCacheDependency()
        {
            return new string[] { $"nodes|{SiteContext.CurrentSiteName}|{ExamplePageType.TYPEINFO}|all" };
        }
    }
}