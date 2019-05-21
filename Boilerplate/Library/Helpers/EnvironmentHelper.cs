using CMS.Helpers;
using CMS.Membership;
using CMS.SiteProvider;
using Kentico.Content.Web.Mvc;
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
        public static bool PreviewEnabled
        {
            get
            {
                try
                {
                    return HttpContext.Current.Kentico().Preview().Enabled;
                } catch(InvalidOperationException)
                {
                    // This occurs only on the Owin Authentication calls due to the Dynamic route handler
                    return false;
                }
            }
        }

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

        public static UserInfo AuthenticatedUser(IPrincipal User)
        {
            string Username = (User != null && User.Identity != null ? User.Identity.Name : "public");
            return CacheHelper.Cache<UserInfo>(cs =>
            {
                return UserInfoProvider.GetUserInfo(Username);
            }, new CacheSettings(CacheHelper.CacheMinutes(SiteContext.CurrentSiteName), "AuthenticatedUser", Username));
        }

    }
}