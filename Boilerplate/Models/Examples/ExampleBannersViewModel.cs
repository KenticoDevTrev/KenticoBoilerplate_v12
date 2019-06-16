using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Models.Examples
{
    public class ExampleBannersViewModel
    {
        public IEnumerable<ExampleBannersBanner> BannerNameUrlsList { get; set; }
    }

    public class ExampleBannersBanner
    {
        public string BannerName { get; set; }
        public string BannerUrl { get; set; }
        public ExampleBannersBanner()
        {

        }
    }
}