using Kentico.Caching.Example;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kentico.Caching.Example
{
    public interface IExamplePageTypeRepository : IRepository
    {
        IEnumerable<ExamplePageTypeModel> GetExamplePages();

        ExamplePageTypeModel GetExamplePage(int ID);

        IEnumerable<string> GetExamplePagesCacheDependency();

        IEnumerable<string> GetExamplePageCacheDependency(int ID);
    }
}