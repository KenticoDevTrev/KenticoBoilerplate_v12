using Boilerplate.Services.Interfaces;
using CMS.Base;
using CMS.DocumentEngine;
using CMS.Localization;
using KMVCHelper;
using Models.Examples;
using System.Collections.Generic;
using System.Web;

namespace Boilerplate.Services.Implementation
{
    public class ExampleService : IExampleService
    {
        public ExampleService()
        {

        }

        /// <summary>
        /// Simply gets the current node using the absolute path of the HttpContext.
        /// Why abstract? Testability, now you can create a mock IExampleService and return an expected TreeNode instead of needing an actual HttpContext
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public ITreeNode GetCurrentNode()
        {
            var absolutePath = HttpContext.Current.Request.Url.AbsolutePath;
            TreeNode FoundNode = DocumentQueryHelper.GetNodeByAliasPath(absolutePath);
            return FoundNode;
        }

        /// <summary>
        /// Gets a list of `Demo.Banner` items under the `Node` passed in.
        /// Why abstract? Testability and reusability.
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public IEnumerable<ExampleBannersBanner> GetBannersFromNode(ITreeNode node)
        {
            var bannerList = new List<ExampleBannersBanner>();
            foreach (TreeNode Banner in DocumentQueryHelper.RepeaterQuery(ClassNames: "Demo.Banner", RelationshipName: "Banners", RelationshipWithNodeGuid: node.NodeGUID))
            {
                bannerList.Add(new ExampleBannersBanner()
                {
                    BannerName = Banner.GetValue<string>("BannerName", ""),
                    BannerUrl = Banner.GetValue<string>("BannerImage", "")
                });
            }
            return bannerList;
        }

        /// <summary>
        /// Gets a list of `SubNav` items under the `Node` passed in.
        /// Why abstract? Testability and reusability.
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public IEnumerable<SubNav> GetSubNavFromNode(ITreeNode node)
        {
            var subnavList = new List<SubNav>();
            foreach (TreeNode Node in DocumentQueryHelper.RepeaterQuery(
                   Path: node.NodeAliasPath + "/%",
                   ClassNames: "CMS.MenuItem",
                   OrderBy: "NodeLevel, NodeOrder",
                   Columns: "MenuItemName,NodeAliasPath"
                   ))
            {
                subnavList.Add(new SubNav()
                {
                    LinkText = Node.GetValue("MenuItemName", ""),
                    // You have to decide what your URL will be, for us our URLs = NodeAliasPath
                    LinkUrl = Node.NodeAliasPath
                });
            }
            return subnavList;
        }

        /// <summary>
        /// Gets a list of `SubNav` items under the `Node` passed in.
        /// Why abstract? Testability and reusability.
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public IEnumerable<SubNav> GetSubNavFromAliasPath(string nodeAliasPath, CultureInfo cultureInfo, ISiteInfo siteInfo)
        {
            var subnavList = new List<SubNav>();
            foreach (TreeNode Node in DocumentQueryHelper.RepeaterQuery(
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
                    LinkText = Node.GetValue("MenuItemName", ""),
                    // You have to decide what your URL will be, for us our URLs = NodeAliasPath
                    LinkUrl = Node.NodeAliasPath
                });
            }
            return subnavList;
        }
    }
}