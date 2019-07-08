using Kentico.Forms.Web.Mvc;
using Kentico.PageBuilder.Web.Mvc.PageTemplates;

namespace Models.TemplatedPage
{
    public class TemplatedPageCustomProperties : IPageTemplateProperties
    {
        [EditingComponent(CheckBoxComponent.IDENTIFIER, Order = 0, Label = "Show Hello World")]
        public bool ShowHelloWorld { get; set; } = true;

        [EditingComponent(TextInputComponent.IDENTIFIER, Order = 0, Label = "Custom Hello World Text")]
        public string HelloWorldText { get; set; } = "Hello World";
    }
}