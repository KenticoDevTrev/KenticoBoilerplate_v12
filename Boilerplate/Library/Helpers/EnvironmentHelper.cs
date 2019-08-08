using CMS.Base;
using CMS.Helpers;
using CMS.Membership;
using CMS.SiteProvider;
using Kentico.Content.Web.Mvc;
using Kentico.PageBuilder.Web.Mvc;
using Kentico.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
namespace KMVCHelper
{
    public class EnvironmentHelper
    {
        /// <summary>
        /// Returns if the Preview is Enabled or not
        /// </summary>
        public static bool PreviewEnabled
        {
            get
            {
                try
                {
                    return HttpContext.Current.Kentico().Preview().Enabled;
                }
                catch (InvalidOperationException)
                {
                    // This occurs only on the Owin Authentication calls due to the Dynamic route handler
                    return false;
                }
            }
        }

        /// <summary>
        /// Returns true if Page Builder is enabled and the page context is set.
        /// </summary>
        public static bool PageBuilderEnabled
        {
            get
            {
                try
                {
                    return HttpContext.Current.Kentico().PageBuilder().EditMode && HttpContext.Current.Kentico().PageBuilder().PageIdentifier > 0;
                }
                catch (InvalidOperationException)
                {
                    // This occurs only on the Owin Authentication calls due to the Dynamic route handler
                    return false;
                }
            }
        }

        /// <summary>
        /// Gets the Preview Culture
        /// </summary>
        public static string PreviewCulture
        {
            get
            {
                try
                {
                    return HttpContext.Current.Kentico().Preview().CultureName;
                }
                catch (InvalidOperationException)
                {
                    // This occurs only on the Owin Authentication calls due to the Dynamic route handler
                    return "en-US";
                }
            }
        }

        /// <summary>
        /// Uses MembershipContext.AuthenticatedUser
        /// </summary>
        /// <returns></returns>
        public static UserInfo AuthenticatedUser()
        {
            return MembershipContext.AuthenticatedUser;
        }

        /// <summary>
        /// Uses the given IPrincipleUser to get the current user.
        /// </summary>
        /// <param name="User"></param>
        /// <returns></returns>
        public static UserInfo AuthenticatedUser(IPrincipal User)
        {
            string Username = (User != null && User.Identity != null ? User.Identity.Name : "public");
            return CacheHelper.Cache<UserInfo>(cs =>
            {
                return UserInfoProvider.GetUserInfo(Username);
            }, new CacheSettings(CacheHelper.CacheMinutes(SiteContext.CurrentSiteName), "AuthenticatedUser", Username));
        }

        /// <summary>
        /// Gets the Url requested, handling any Virtual Directories
        /// </summary>
        /// <param name="Request">The Request</param>
        /// <returns>The Url for lookup</returns>
        public static string GetUrl(HttpRequestBase Request)
        {
            return GetUrl(Request.Url.AbsolutePath, Request.ApplicationPath);
        }

        /// <summary>
        /// Gets the Url requested, handling any Virtual Directories
        /// </summary>
        /// <param name="Request">The Request</param>
        /// <returns>The Url for lookup</returns>
        public static string GetUrl(HttpRequest Request)
        {
            return GetUrl(Request.Url.AbsolutePath, Request.ApplicationPath);
        }

        /// <summary>
        /// Gets the Url requested, handling any Virtual Directories
        /// </summary>
        /// <param name="Request">The Request</param>
        /// <returns>The Url for lookup</returns>
        public static string GetUrl(IRequest Request)
        {
            return GetUrl(Request.Url.AbsolutePath, HttpContext.Current.Request.ApplicationPath);
        }

        /// <summary>
        /// Removes Application Path from Url if present and ensures starts with /
        /// </summary>
        /// <param name="Url">The Url (Relative)</param>
        /// <param name="ApplicationPath"></param>
        /// <returns></returns>
        public static string GetUrl(string RelativeUrl, string ApplicationPath)
        {
            // Remove Application Path from Relative Url if it exists at the beginning
            if (!string.IsNullOrWhiteSpace(ApplicationPath) && ApplicationPath != "/" && RelativeUrl.ToLower().IndexOf(ApplicationPath.ToLower()) == 0)
            {
                RelativeUrl = RelativeUrl.Substring(ApplicationPath.Length);
            }

            return "/" + RelativeUrl.Trim("/~".ToCharArray()).Split("?#:".ToCharArray())[0];
        }

    }
}