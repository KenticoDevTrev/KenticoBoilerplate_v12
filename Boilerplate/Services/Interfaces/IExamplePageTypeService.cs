using Models.Examples;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCCaching.Kentico.Example
{
    public interface IExamplePageTypeService : IService
    {
        ExamplePageTypeModel AllCaps(ExamplePageTypeModel model);
        
    }
}