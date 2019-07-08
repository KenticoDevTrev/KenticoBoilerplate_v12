using Controllers.PageTemplates;
using Kentico.PageBuilder.Web.Mvc.PageTemplates;
using Models.TemplatedPage;

[assembly: RegisterPageTemplate("TemplatedPageBasicTemplate", "Basic Template", Description = "Basic Template example", IconClass = "icon-layout")]
[assembly: RegisterPageTemplate("TemplatedPageBasicTemplateCustomView", "Basic Template (Custom View)", customViewName: "PageTemplates/_TemplatedPageCustomView", Description = "Basic template example with custom View Name", IconClass = "icon-rectangle-a")]
[assembly: RegisterPageTemplate("TemplatedPageBasicWithProperties", "Template with Properties", typeof(TemplatedPageCustomProperties), Description = "Template with Properties", IconClass = "icon-cogwheel")]
[assembly: RegisterPageTemplate("TemplatedPageBasicWithCustomController", typeof(TemplatedPageCustomControllerController), "Template with Custom Controller", Description = "Template with a Custom Controller", IconClass = "icon-cogwheel-square")]
[assembly: RegisterPageTemplate("TemplatedPageCustomControllerWithProperties", typeof(TemplatedPageCustomControllerWithPropertiesController), "Template with Custom Controller and Properties", Description = "Template with a Custom Controller and Properties", IconClass = "icon-cogwheels")]