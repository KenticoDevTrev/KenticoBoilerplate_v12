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
        IEnumerable<ExampleModuleClassModel> GetExampleModuleClasses();

        ExampleModuleClassModel GetExampleModuleClass(int ID);
    }
}