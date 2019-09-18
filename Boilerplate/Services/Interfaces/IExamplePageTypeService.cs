using Models.Examples;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kentico.Caching.Example
{
    public interface IExamplePageTypeService : IService
    {
        ExamplePageTypeModel AllCaps(ExamplePageTypeModel model);
        
    }
}