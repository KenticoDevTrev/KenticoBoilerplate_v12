using Kentico.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCCaching.Kentico.Example
{
    public class ExampleCacheKey : IOutputCacheKey
    {
        public string Name => "Example";

        public string GetVaryByCustomString(HttpContextBase context, string custom)
        {
            // Here is where you would generate the string that will be part of the Cache's name
            return $"{Name}=HelloWorld";
        }
    }
}