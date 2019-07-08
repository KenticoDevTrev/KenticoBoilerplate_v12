using Kentico.PageBuilder.Web.Mvc.PageTemplates;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Controllers.PageTemplates
{
    public class TemplatedPageTemplateFilter : IPageTemplateFilter
    {
        public IEnumerable<PageTemplateDefinition> Filter(IEnumerable<PageTemplateDefinition> pageTemplates, PageTemplateFilterContext context)
        {
            // Applies filtering to a collection of page templates based on the page type of the currently edited page
            if (context.PageType.Equals("Example.TemplatedPage", StringComparison.InvariantCultureIgnoreCase))
            {
                // Filters the collection to only contain filters allowed for landing pages
                return pageTemplates.Where(t => GetTemplatedPageTemplates().Contains(t.Identifier));
            }

            // Excludes all landing page templates from the collection if the context does not match this filter
            // Assumes that the categories of page templates are mutually exclusive
            return pageTemplates.Where(t => !GetTemplatedPageTemplates().Contains(t.Identifier));
        }

        // Gets all page templates that are allowed for landing pages
        public IEnumerable<string> GetTemplatedPageTemplates() => new string[] { "TemplatedPageBasicTemplate", "TemplatedPageBasicTemplateCustomView", "TemplatedPageBasicWithProperties", "TemplatedPageBasicWithCustomController", "TemplatedPageCustomControllerWithProperties" };
    }
}