using Kentico.Content.Web.Mvc;
using Kentico.PageBuilder.Web.Mvc;
using Kentico.Web.Mvc;
public class ApplicationConfig
{
    public static void RegisterFeatures(IApplicationBuilder builder)
    {
        // Enable required Kentico features
        builder.UsePreview();
        // Registers the Page Builder with the default section we created
        builder.UsePageBuilder(new PageBuilderOptions()
        {
            // Specifies a default section for the page builder feature
            DefaultSectionIdentifier = "Sections.DefaultSection",
            // Disables the system's built-in 'Default' section
            RegisterDefaultSection = false
        });
    }
}
