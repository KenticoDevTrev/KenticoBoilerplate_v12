using CMS.DataEngine;
using CMS.Helpers;
using CMS.SiteProvider;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web;

namespace Kentico.Caching
{
    /// <summary>
    /// Implementation of ICacheHelper using Kentico's CacheHelper functions
    /// </summary>
    public class KenticoCacheHelper : ICacheHelper
    {

        public T Cache<T>(Func<T> Func, string KeyName, IEnumerable<string> Dependencies, TimeSpan CacheDuration)
        {
            return CacheHelper.Cache<T>(cs =>
            {
                if(cs.Cached)
                {
                    cs.CacheDependency = CacheHelper.GetCacheDependency(Dependencies.ToArray());
                }
                return Func.Invoke();
            }, new CacheSettings(CacheDuration.Minutes, KeyName));
        }

        public TimeSpan CacheDuration(string ObjectIdentifier = "")
        {
            var value = ConfigurationManager.AppSettings["RepositoryCacheItemDuration"];
            var seconds = 0;

            if (Int32.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out seconds) && seconds > 0)
            {
                return TimeSpan.FromSeconds(seconds);
            }
            else
            {
                try
                {
                    return TimeSpan.FromMinutes(SettingsKeyInfoProvider.GetIntValue("CMSCacheMinutes", new SiteInfoIdentifier(SiteContext.CurrentSiteName)));
                }
                catch (Exception)
                {
                    return TimeSpan.Zero;
                }
            }
        }

        public void TouchKey(string KeyName)
        {
            CacheHelper.TouchKey(KeyName);
        }

        public void TouchKeys(IEnumerable<string> KeyNames)
        {
            CacheHelper.TouchKeys(KeyNames);
        }
    }
}