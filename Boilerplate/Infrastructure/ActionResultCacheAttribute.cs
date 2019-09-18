using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Kentico.Caching
{
    /// <summary>
    /// Caches the result of an action method (Useful for "Donut" caching, this will cache the ActionResult Logic and not the View)
    /// https://www.davidhaney.io/custom-asp-net-mvc-action-result-cache-attribute/
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class ActionResultCacheAttribute : ActionFilterAttribute
    {
        private static readonly Dictionary<string, string[]> _varyByParamsSplitCache = new Dictionary<string, string[]>();
        private static readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();
        private static readonly MemoryCache _cache = new MemoryCache("ActionResultCacheAttribute");

        /// <summary>
        /// The comma separated parameters to vary the caching by.
        /// </summary>
        public string VaryByParam { get; set; }

        /// <summary>
        /// Custom String to vary by
        /// </summary>
        public string VaryByCustom { get; set; }

        /// <summary>
        /// Vary by Header values, semi-colon separated
        /// </summary>
        public string VaryByHeader { get; set; }

        /// <summary>
        /// Vary by Cookie values, semi-colon separated
        /// </summary>
        public string VaryByCookie { get; set; }


        /// <summary>
        /// The duration to cache before expiration, in seconds.
        /// </summary>
        public int Duration { get; set; }

        /// <summary>
        /// Occurs when an action is executing.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Create the cache key
            var cacheKey = CreateCacheKey(filterContext.RouteData.Values, filterContext.ActionParameters);

            // Try and get the action method result from cache
            var result = _cache.Get(cacheKey) as ActionResult;
            if (result != null)
            {
                // Set the result
                filterContext.Result = result;
                return;
            }

            // Store to HttpContext Items
            filterContext.HttpContext.Items["__actionresultcacheattribute_cachekey"] = cacheKey;
        }

        /// <summary>
        /// Occurs when an action has executed.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            // Don't cache errors
            if (filterContext.Exception != null)
            {
                return;
            }

            // Get the cache key from HttpContext Items
            var cacheKey = filterContext.HttpContext.Items["__actionresultcacheattribute_cachekey"] as string;
            if (string.IsNullOrWhiteSpace(cacheKey))
            {
                return;
            }

            if (Duration != 0)
            {
                _cache.Add(cacheKey, filterContext.Result, DateTime.UtcNow.AddSeconds(Duration));
                return;
            }

            // Default to 1 hour
            _cache.Add(cacheKey, filterContext.Result, DateTime.UtcNow.AddSeconds(60 * 60));
        }

        /// <summary>
        /// Creates the cache key.
        /// </summary>
        /// <param name="routeValues">The route values.</param>
        /// <returns>The cache key.</returns>
        private string CreateCacheKey(RouteValueDictionary routeValues, IDictionary<string, object> actionParameters)
        {
            // Create the cache key prefix as the controller and action method
            var sb = new StringBuilder(routeValues["controller"].ToString());
            sb.Append("_").Append(routeValues["action"].ToString());

            if (!string.IsNullOrWhiteSpace(VaryByParam))
            {

                // Append the cache key from the vary by parameters
                object varyByParamObject = null;
                string[] varyByParamsSplit = null;
                bool gotValue = false;

                _lock.EnterReadLock();
                try
                {
                    gotValue = _varyByParamsSplitCache.TryGetValue(VaryByParam, out varyByParamsSplit);
                }
                finally
                {
                    _lock.ExitReadLock();
                }

                if (!gotValue)
                {
                    _lock.EnterWriteLock();
                    try
                    {
                        varyByParamsSplit = VaryByParam.Split(",; ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                        _varyByParamsSplitCache[VaryByParam] = varyByParamsSplit;
                    }
                    finally
                    {
                        _lock.ExitWriteLock();
                    }
                }

                foreach (var varyByParam in varyByParamsSplit)
                {
                    // Skip invalid parameters
                    if (!actionParameters.TryGetValue(varyByParam, out varyByParamObject))
                    {
                        continue;
                    }

                    // Sometimes a parameter will be null
                    if (varyByParamObject == null)
                    {
                        continue;
                    }

                    sb.Append("_").Append(varyByParamObject.ToString());
                }
            }

            // Handle VaryByCustom
            if(!string.IsNullOrWhiteSpace(VaryByCustom))
            {
                sb.Append("_"+ HttpContext.Current.ApplicationInstance.GetVaryByCustomString(HttpContext.Current, VaryByCustom));
            }

            if (!string.IsNullOrWhiteSpace(VaryByHeader))
            {
                sb.Append("_Header");
                foreach (string Header in VaryByHeader.Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)) {
                    string[] vals = HttpContext.Current.Request.Headers[Header] != null ? HttpContext.Current.Request.Headers.GetValues(Header) : new string[] { };
                    
                    sb.Append($"_{Header}={string.Join("|", vals)}");
                }
            }

            if (!string.IsNullOrWhiteSpace(VaryByCookie))
            {
                sb.Append("_Cookie");
                foreach (string Cookie in VaryByCookie.Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries))
                {
                    string val = HttpContext.Current.Request.Cookies[Cookie] != null ? HttpContext.Current.Request.Cookies[Cookie].Value : "";

                    sb.Append($"_{Cookie}={val}");
                }
            }

            return sb.ToString();
        }
    }
}