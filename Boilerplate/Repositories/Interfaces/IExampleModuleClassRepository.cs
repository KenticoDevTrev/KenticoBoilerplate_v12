using Kentico.Caching;
using Kentico.Caching.Example;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kentico.Caching.Example
{
    public interface IExampleModuleClassRepository : IRepository
    {
        #region "These are abstracted out to Generic Models"

        IEnumerable<ExampleModuleClassModel> GetExampleModuleClasses();

        ExampleModuleClassModel GetExampleModuleClass(int ID);

        IEnumerable<string> GetExampleModuleClassesCacheDependency();

        IEnumerable<string> GetExampleModuleClassCacheDependency(int ID);

        #endregion

        #region "These are returning BaseInfo/TreeNode Models"

        ExampleModuleClassInfo GetExampleModuleClass_BaseInfo(int ID);

        IEnumerable<ExampleModuleClassInfo> GetExampleModuleClasses_BaseInfo();

        #endregion
    }
}