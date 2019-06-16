using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Models.Examples
{
    public class ExampleMVCWebPartsViewModel
    {
        public IEnumerable<SubNav> SubNavigation { get; set; }
        public ExampleMVCWebPartsViewModel()
        {
            SubNavigation = new List<SubNav>();
        }
    }

    public class ExampleMVCWebPartsSubNavs
    {
        public IEnumerable<SubNav> SubNavigation { get; set; }
        public ExampleMVCWebPartsSubNavs()
        {
            SubNavigation = new List<SubNav>();
        }
    }

    public class SubNav
    {
        public string LinkText { get; set; }
        public string LinkUrl { get; set; }

        public SubNav()
        {

        }
    }
}