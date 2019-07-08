using CMS.DocumentEngine;
using CMS.Helpers;
using CMS.Localization;
using CMS.SiteProvider;
using Kentico.PageBuilder.Web.Mvc;
using Kentico.PageBuilder.Web.Mvc.PageTemplates;
using Kentico.Web.Mvc;
using KMVCHelper;
using System.Linq;
using System.Web.Mvc;

namespace Controllers
{
    public class TemplatedBaseController : PageTemplateController
    {
        public TemplatedBaseController()
        {

        }

        /// <summary>
        /// Sets in the Viewbag the CurrentDocument (TreeNode), CurrentSite (SiteInfo), and CurrentCulture (CultureInfo)
        /// </summary>
        /// <param name="DocumentID">The Document ID</param>
        /// <param name="CultureCode">The culture if you wish it to not be based on the found Document's</param>
        /// <param name="SiteName">The SiteName if you wish it to not be based on the Document's</param>
        public void SetContext(int DocumentID, string CultureCode = null, string SiteName = null)
        {
            TreeNode DocumentNodeForClass = new DocumentQuery().WhereEquals("DocumentID", DocumentID).FirstOrDefault();
            TreeNode DocumentContext = null;
            if (DocumentNodeForClass != null)
            {
                // Set Page Builder Context
                HttpContext.Kentico().PageBuilder().Initialize(DocumentID);

                CacheableDocumentQuery RepeaterQuery = new CacheableDocumentQuery(DocumentNodeForClass.ClassName);
                RepeaterQuery.WhereEquals("DocumentID", DocumentID);
                RepeaterQuery.CacheItemNameParts.Add("documentid|" + DocumentID);

                if (EnvironmentHelper.PreviewEnabled)
                {
                    RepeaterQuery.LatestVersion(true);
                    RepeaterQuery.Published(false);
                }
                else
                {
                    RepeaterQuery.PublishedVersion(true);
                }
                DocumentContext = RepeaterQuery.GetTypedResult().Items.FirstOrDefault();
            }
            if (DocumentContext != null)
            {
                ViewBag.CurrentDocument = DocumentContext;
                if (string.IsNullOrWhiteSpace(SiteName))
                {
                    SiteName = DocumentContext.NodeSiteName;
                }
                if (string.IsNullOrWhiteSpace(CultureCode))
                {
                    CultureCode = DocumentContext.DocumentCulture;
                }
            }
            if (!string.IsNullOrWhiteSpace(SiteName))
            {
                ViewBag.CurrentSite = CacheHelper.Cache<SiteInfo>(cs =>
                {
                    SiteInfo SiteObj = SiteInfoProvider.GetSiteInfo(SiteName);
                    if (cs.Cached)
                    {
                        cs.CacheDependency = CacheHelper.GetCacheDependency("cms.site|byid|" + SiteObj.SiteID);
                    }
                    return SiteObj;
                }, new CacheSettings(CacheHelper.CacheMinutes(SiteName), "GetSiteInfo", SiteName));
            }
            if (!string.IsNullOrWhiteSpace(CultureCode))
            {
                ViewBag.CurrentCulture = CacheHelper.Cache<CultureInfo>(cs =>
                {
                    CultureInfo CultureObj = CultureInfoProvider.GetCultureInfo(CultureCode);
                    if (cs.Cached)
                    {
                        cs.CacheDependency = CacheHelper.GetCacheDependency(CultureInfo.TYPEINFO.ObjectClassName + "|byid|" + CultureObj.CultureID);
                    }
                    return CultureObj;
                }, new CacheSettings(CacheHelper.CacheMinutes(SiteName), "GetCultureInfo", CultureCode)); ;
            }
        }
    }
}