using System.Web.Mvc;

using Kentico.PageBuilder.Web.Mvc;
using Controllers.Sections;
[assembly: RegisterSection("Sections.ThreeColumnSection", typeof(ThreeColumnSectionController), "{$section.threecolumn.name$}", Description = "{$section.threecolumn.description$}", IconClass = "icon-l-cols-3")]

namespace Controllers.Sections
{
    public class ThreeColumnSectionController : Controller
    {
        // GET: ThreeColumnSection
        public ActionResult Index()
        {
            return PartialView("Sections/_ThreeColumnSection");
        }
    }
}