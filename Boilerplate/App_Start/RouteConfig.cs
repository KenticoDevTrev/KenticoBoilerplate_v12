using System.Web.Mvc;
using System.Web.Routing;
using KMVCHelper;
using Kentico.Web.Mvc;

public class RouteConfig
{
    public static void RegisterRoutes(RouteCollection routes)
    {
        routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

        // Maps routes to Kentico HTTP handlers and features enabled in ApplicationConfig.cs
        // Always map the Kentico routes before adding other routes. Issues may occur if Kentico URLs are matched by a general route, for example images might not be displayed on pages
        routes.Kentico().MapRoutes();

        // Redirect to administration site if the path is "admin"
        routes.MapRoute(
            name: "Admin",
            url: "admin",
            defaults: new { controller = "AdminRedirect", action = "Index" }
        );

        // If a normal MVC Route is found and it has priority, it will take it, otherwise it will bypass.
        var route = routes.MapRoute(
             name: "DefaultIfPriority",
             url: "{controller}/{action}/{id}",
             defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
             constraints: new { ControlIsPriority = new KMVCRouteOverPathPriorityConstraint() }
         );

        // If the Page is found, will handle the routing dynamically
        route = routes.MapRoute(
            name: "CheckByUrl",
            url: "{*url}",
            // Defaults are if it can't find a controller based on the pages
            defaults: new { defaultcontroller = "HttpErrors", defaultaction = "Index" },
            constraints: new { PageFound = new KMVCPageFoundConstraint(true) }
        );
        route.RouteHandler = new KMVCDynamicRouteHandler();

        // This will again look for matching routes or node alias paths, this time it won't care if the route is priority or not.
        routes.MapRoute(
             name: "Default",
             url: "{controller}/{action}/{id}",
             defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
        );

        // Finally, 404
        routes.MapRoute(
            name: "PageNotFound",
            url: "{*url}",
            defaults: new { controller = "HttpErrors", action = "Index" }
            );
    }
}