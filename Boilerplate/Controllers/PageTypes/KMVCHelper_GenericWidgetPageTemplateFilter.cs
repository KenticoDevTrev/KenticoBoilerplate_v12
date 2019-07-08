using Kentico.PageBuilder.Web.Mvc.PageTemplates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Controllers.PageTemplates
{
    public class KMVCHelper_GenericWidgetPageTemplateFilter : IPageTemplateFilter
    {
        public IEnumerable<PageTemplateDefinition> Filter(IEnumerable<PageTemplateDefinition> pageTemplates, PageTemplateFilterContext context)
        {
            // only add empty option if there is 1 non empty template remaining, so user has to choose.
            if (context.PageType.Equals("KMVCHelper.GenericWidgetPage", StringComparison.InvariantCultureIgnoreCase))
            {
                return pageTemplates;
            }
            else
            {
                // Remove the empty template as an option
                return pageTemplates.Where(t => !GetTemplates().Contains(t.Identifier));
            }
        }

        // Gets all page templates that are allowed for landing pages
        public IEnumerable<string> GetTemplates() => new string[] { "Blank.Widget" };
    }
}