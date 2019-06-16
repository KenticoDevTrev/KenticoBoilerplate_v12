using KMVCHelper;
using System.Web.Optimization;

public class Bundles
{
    public static void RegisterBundles(BundleCollection bundles)
    {
        RegisterJqueryBundle(bundles);
        RegisterJqueryUnobtrusiveAjaxBundle(bundles);
        RegisterJqueryValidationBundle(bundles);
    }

    private static void RegisterJqueryBundle(BundleCollection bundles)
    {
        bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
             "~/Kentico/Scripts/jquery-3.3.1.js"));
    }

    private static void RegisterJqueryValidationBundle(BundleCollection bundles)
    {
        bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                    "~/Content/js/jquery.validate/jquery.validate-vsdoc.js",
                    "~/Content/js/jquery.validate/jquery.validate.js",
                    "~/Content/js/jquery.validate/jquery.validate.unobtrusive.js"));
    }


    private static void RegisterJqueryUnobtrusiveAjaxBundle(BundleCollection bundles)
    {
        var bundle = new ScriptBundle("~/bundles/jquery-unobtrusive-ajax")
            .Include("~/Kentico/Scripts/jquery.unobtrusive-ajax.js");

        bundles.Add(bundle);
    }
}