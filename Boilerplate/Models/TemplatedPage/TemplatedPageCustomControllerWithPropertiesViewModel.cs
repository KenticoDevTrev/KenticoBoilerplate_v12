using Kentico.PageBuilder.Web.Mvc.PageTemplates;

namespace Models.TemplatedPage
{
    public class TemplatedPageCustomControllerWithPropertiesViewModel : IPageTemplateProperties
    {
        public string Message { get; set; }
        public bool ShowHelloWorld { get; set; } = true;

        public string HelloWorldText { get; set; } = "Hello World";
    }
}