using System.Web.Mvc;

using Kentico.PageBuilder.Web.Mvc;
using Controllers.Sections;
[assembly: RegisterSection("Sections.TwoColumnSection", typeof(TwoColumnSectionController), "{$section.twocolumn.name$}", Description = "{$section.twocolumn.description$}", IconClass = "icon-l-cols-2")]

namespace Controllers.Sections
{
    public class TwoColumnSectionController : Controller
    {
        // GET: TwoColumnSection
        public ActionResult Index()
        {
            return PartialView("Sections/_TwoColumnSection");
        }
    }
}