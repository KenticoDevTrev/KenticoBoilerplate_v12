using Kentico.PageBuilder.Web.Mvc.PageTemplates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

[assembly: RegisterPageTemplate("Empty.Template", "No Template", customViewName: "_EmptyTemplate", Description ="No Template (Use standard Dynamic Routing)", IconClass = "icon-modal-close")]

[assembly: RegisterPageTemplate("Blank.Widget", "Blank Widget Page", customViewName: "_BlankWidgetTemplate", Description = "Blank page with a widget zone")]