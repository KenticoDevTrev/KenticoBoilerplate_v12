using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kentico.Caching.Example
{
    public class KenticoExampleModuleClassRepository : IExampleModuleClassRepository
    {
        [CacheDependency("example.examplemoduleclass|byid|{0}")]
        public ExampleModuleClassModel GetExampleModuleClass(int ID)
        {
            var ExampleModuleClass = ExampleModuleClassInfoProvider.GetExampleModuleClassInfo(ID);

            return new ExampleModuleClassModel()
            {
                Name = ExampleModuleClass?.ExampleModuleClassName
            };
        }

        public IEnumerable<string> GetExampleModuleClassCacheDependency(int ID)
        {
            return new string[] { $"example.examplemoduleclass|byid|{ID}"};
        }

        public IEnumerable<ExampleModuleClassModel> GetExampleModuleClasses()
        {
            var ModelClasses = ExampleModuleClassInfoProvider.GetExampleModuleClasses()
                .ToList();

            return ModelClasses.Select(x =>
            {
                return new ExampleModuleClassModel()
                {
                    Name = x.ExampleModuleClassName
                };
            });
        }

        public IEnumerable<string> GetExampleModuleClassesCacheDependency()
        {
            return new string[] { $"example.examplemoduleclass|all" };
        }
    }
}
