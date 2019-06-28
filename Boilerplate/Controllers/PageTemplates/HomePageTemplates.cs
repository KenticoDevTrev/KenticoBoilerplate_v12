using Boilerplate.Controllers.PageTemplates;
using CMS.DocumentEngine;
using Kentico.PageBuilder.Web.Mvc.PageTemplates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

[assembly: RegisterPageTemplate("Home.Template1", "Template 1", customViewName: "_HomeTemplate1")]
[assembly: RegisterPageTemplate("Home.Template2", "Template 2", customViewName: "_HomeTemplate2")]