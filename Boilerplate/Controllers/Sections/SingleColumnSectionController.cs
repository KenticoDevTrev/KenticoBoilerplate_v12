using System.Web.Mvc;

using Kentico.PageBuilder.Web.Mvc;
using Controllers.Sections;

[assembly: RegisterSection("Sections.SingleColumnSection", typeof(SingleColumnSectionController), "{$section.singlecolumn.name$}", Description = "{$section.singlecolumn.description$}", IconClass = "icon-square")]

namespace Controllers.Sections
{
    public class SingleColumnSectionController : Controller
    {
        // GET: SingleColumnSection
        public ActionResult Index()
        {
            return PartialView("Sections/_SingleColumnSection");
        }
    }
}