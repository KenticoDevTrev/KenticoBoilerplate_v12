using MVCCaching.Kentico.Example;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCCaching.Kentico.Example
{
    public interface IExamplePageTypeRepository : IRepository
    {
        #region "These are abstracted out to Generic Models"

        IEnumerable<ExamplePageTypeModel> GetExamplePages();

        ExamplePageTypeModel GetExamplePage(int ID);

        IEnumerable<string> GetExamplePagesCacheDependency();

        IEnumerable<string> GetExamplePageCacheDependency(int ID);

        #endregion

        #region "These are returning BaseInfo/TreeNode Models"

        IEnumerable<ExamplePageType> GetExamplePages_TreeNodes();

        ExamplePageType GetExamplePage_TreeNode(int ID);

        #endregion  
    }
}