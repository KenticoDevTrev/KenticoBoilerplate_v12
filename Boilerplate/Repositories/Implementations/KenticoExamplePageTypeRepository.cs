using CMS.SiteProvider;
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

        public KenticoExamplePageTypeRepository(string cultureName, bool latestVersionEnabled)
        {
            mCultureName = cultureName;
            mLatestVersionEnabled = latestVersionEnabled;
        }

        /// <summary>
        /// Gets the specific Example Page.  Adding a specific cache dependency
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [CacheDependency("node|byid|{0}")]
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

        public IEnumerable<string> GetExamplePageCacheDependency(int ID)
        {
            return new string[] { $"nodeid|{ID}" };
        }

        /// <summary>
        /// Gets All the example pages, in this case the default Cache Dependency will be all the Example Pages
        /// </summary>
        /// <returns></returns>        
        public IEnumerable<ExamplePageTypeModel> GetExamplePages()
        {
            // Get the Pages
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
                        ID=x.NodeID
                    };
                })
                .ToList();
        }

        public IEnumerable<string> GetExamplePagesCacheDependency()
        {
            return new string[] { $"nodes|{SiteContext.CurrentSiteName}|{ExamplePageType.TYPEINFO}|all" };
        }
    }
}