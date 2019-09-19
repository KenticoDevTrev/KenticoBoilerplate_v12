using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models.Examples;

namespace MVCCaching.Kentico.Example
{
    public class ExamplePageTypeService : IExamplePageTypeService
    {
        public ExamplePageTypeModel AllCaps(ExamplePageTypeModel model)
        {
            model.Name = model.Name.ToUpper();
            return model;
        }
    }
}