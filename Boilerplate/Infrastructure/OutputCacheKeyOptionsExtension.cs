﻿using Kentico.Caching.Example;
using Kentico.Web.Mvc;


namespace Kentico.Caching.Example
{
    public static class OutputCacheKeyOptionsExtensions
    {
        // Varies the output cache based on the contact's gender
        public static IOutputCacheKeyOptions VaryByExample(this IOutputCacheKeyOptions options)
        {
            // Adds the ContactGenderOutputCacheKey to the options object
            options.AddCacheKey(new ExampleCacheKey());

            return options;
        }
    }
}