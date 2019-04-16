using Controllers.Sections;
using Kentico.PageBuilder.Web.Mvc;
using System.Web.Mvc;

[assembly: RegisterSection("Sections.DefaultSection", typeof(DefaultSectionController), "Default section", Description = "A default section with one widget zone.", IconClass = "icon-box")]
namespace Controllers.Sections
{
    public class DefaultSectionController : Controller
    {
        // GET action used to retrieve the section markup
        public ActionResult Index()
        {
            return PartialView("Sections/_DefaultSection");
        }
    }
}