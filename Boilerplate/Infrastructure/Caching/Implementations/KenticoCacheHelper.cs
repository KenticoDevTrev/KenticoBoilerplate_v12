using CMS.DataEngine;
using CMS.Helpers;
using CMS.SiteProvider;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;

namespace MVCCaching.Kentico
{
    /// <summary>
    /// Implementation of ICacheHelper using Kentico's CacheHelper functions
    /// </summary>
    public class KenticoCacheHelper : ICacheHelper
    {
        /// <summary>
        /// Caches the given Function using Kentico's CacheHelper
        /// </summary>
        /// <typeparam name="T">The return type</typeparam>
        /// <param name="Func">The Function to Cache</param>
        /// <param name="KeyName">The Cache Key Name</param>
        /// <param name="Dependencies">Cache Dependencies</param>
        /// <param name="CacheDuration">How long to cache, can use the CacheDuration method if you do not wish to specify</param>
        /// <returns></returns>
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

        /// <summary>
        /// Gets the default Cache Duration.  Looks to the AppSetting RepositoryCacheItemDuration (seconds), or uses the Data Cache Minutes setting in Kentico
        /// </summary>
        /// <param name="ObjectIdentifier"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Clears any cache by the given Key
        /// </summary>
        /// <param name="KeyName">The Cache Key</param>
        public void TouchKey(string KeyName)
        {
            CacheHelper.TouchKey(KeyName);
        }

        /// <summary>
        /// Clears any Caches by the given Keys
        /// </summary>
        /// <param name="KeyNames">The Cache Keys</param>
        public void TouchKeys(IEnumerable<string> KeyNames)
        {
            CacheHelper.TouchKeys(KeyNames);
        }
    }
}