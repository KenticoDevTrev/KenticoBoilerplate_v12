using Kentico.Activities.Web.Mvc;
using Kentico.CampaignLogging.Web.Mvc;
using Kentico.Content.Web.Mvc;
using Kentico.Content.Web.Mvc.Routing;
using Kentico.Newsletters.Web.Mvc;
using Kentico.PageBuilder.Web.Mvc;
using Kentico.Web.Mvc;

public class ApplicationConfig
{
    public static void RegisterFeatures(IApplicationBuilder builder)
    {
        // Enable required Kentico features
        builder.UsePreview();
        // Registers the Page Builder with the default section we created
        builder.UsePageBuilder(new PageBuilderOptions());

        // Enables the alternative URLs feature
        builder.UsePageRouting(new PageRoutingOptions
        {
            EnableAlternativeUrls = true
        });

        // Enable Model-based Data Annotations for localization: 
        //https://docs.kentico.com/k12/multilingual-websites/setting-up-multilingual-websites/localizing-content-on-mvc-sites#LocalizingcontentonMVCsites-Localizingvalidationresultsandmodelproperties
        builder.UseDataAnnotationsLocalization();

		// Enables Cross-origin resource sharing with the admin interface. This is required to allow the Email Marketing application to be aware of newsletter activities such as unsubscription, approvals, opens, and clicks
		//https://docs.kentico.com/k12/on-line-marketing-features/configuring-and-customizing-your-on-line-marketing-features/configuring-email-marketing/handling-newsletter-subscriptions-on-mvc-sites#HandlingnewslettersubscriptionsonMVCsites-Enablingresourcesharing
		builder.UseResourceSharingWithAdministration();

		//Enable Campaign Tracking
		//https://docs.kentico.com/k12/on-line-marketing-features/configuring-and-customizing-your-on-line-marketing-features/tracking-campaigns-on-mvc-sites
		builder.UseCampaignLogger();

		//Allows the site to track automatic activities - External Search and Page View
		//https://docs.kentico.com/k12/on-line-marketing-features/configuring-and-customizing-your-on-line-marketing-features/configuring-activities/enabling-activity-tracking-on-mvc-sites/logging-activities-on-mvc-sites#LoggingactivitiesonMVCsites-Loggingpagerelatedactivities
		builder.UseActivityTracking();

		//Allows tracking of email marketing activities
		//https://docs.kentico.com/k12/on-line-marketing-features/configuring-and-customizing-your-on-line-marketing-features/configuring-email-marketing/enabling-marketing-email-tracking/tracking-marketing-emails-on-mvc-sites
		builder.UseEmailTracking(new EmailTrackingOptions()
		{
			EmailLinkHandlerRouteUrl = CMS.Newsletters.EmailTrackingLinkHelper.DEFAULT_LINKS_TRACKING_ROUTE_HANDLER_URL,
			OpenedEmailHandlerRouteUrl = CMS.Newsletters.EmailTrackingLinkHelper.DEFAULT_OPENED_EMAIL_TRACKING_ROUTE_HANDLER_URL
		});

    }
}
