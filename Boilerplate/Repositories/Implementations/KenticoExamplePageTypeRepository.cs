using CMS.SiteProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCCaching.Kentico.Example
{

    public class KenticoExamplePageTypeRepository : IExamplePageTypeRepository
    {
        private readonly string mCultureName;
        private readonly bool mLatestVersionEnabled;
        private ICacheHelper mCacheHelper;

        #region "These are abstracted out to Generic Models"

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
        /// Gets the specific Example Page.  Adding a specific cache dependency
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [CacheDependency("nodeguid|##SITENAME##|{0}")]
        public ExamplePageTypeModel GetExamplePage(Guid guid)
        {
            // Get the Page
            var Page = ExamplePageTypeProvider.GetExamplePageType(guid, mCultureName, SiteContext.CurrentSiteName)
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
        /// Adds the cache "nodes|sitename|classname|all"
        /// </summary>
        /// <returns></returns>
        [PagesCacheDependency(ExamplePageType.OBJECT_TYPE)]
        public IEnumerable<ExamplePageTypeModel> GetExamplePages()
        {
            var Pages = ExamplePageTypeProvider.GetExamplePageTypes()
                 .LatestVersion(mLatestVersionEnabled)
                 .Published(!mLatestVersionEnabled)
                 .OnSite(SiteContext.CurrentSiteName)
                 .Culture(mCultureName)
                 .CombineWithDefaultCulture()
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
        }

        /// <summary>
        /// If you need to have a custom Cache Dependency, you will need to use custom Caching
        /// </summary>
        /// <returns>All the Example Pages</returns>   
        public IEnumerable<ExamplePageTypeModel> GetExamplePages_CustomCacheDependency()
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

        #endregion

        #region "These are returning BaseInfo/TreeNode Models"

        /// <summary>
        /// No Cache needed as the Interceptor will detect the return type is IEnumerable of TreeNode and add the "nodes|sitename|classname|all" dependency
        /// </summary>
        /// <returns>Get All Example Pages</returns>
        public IEnumerable<ExamplePageType> GetExamplePages_TreeNodes()
        {
            return ExamplePageTypeProvider.GetExamplePageTypes()
                .LatestVersion(mLatestVersionEnabled)
                .Published(!mLatestVersionEnabled)
                .OnSite(SiteContext.CurrentSiteName)
                .Culture(mCultureName)
                .CombineWithDefaultCulture()
                .OrderBy("NodeOrder")
                .ToList();
        }

        /// <summary>
        /// No Cache 'needed' however basic detection of it will simply put the "nodes|sitename|classname|all", so i'm adding one
        /// </summary>
        /// <param name="ID">The NodeID</param>
        /// <returns>The Page</returns>
        [CacheDependency("nodeid|{0}")]
        public ExamplePageType GetExamplePage_TreeNode(int ID)
        {
            return ExamplePageTypeProvider.GetExamplePageType(ID, mCultureName, SiteContext.CurrentSiteName)
                .LatestVersion(mLatestVersionEnabled)
                .Published(!mLatestVersionEnabled)
                .CombineWithDefaultCulture()
                .FirstOrDefault();
        }

        #endregion 

    }
}