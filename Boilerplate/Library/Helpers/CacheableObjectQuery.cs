using CMS.DataEngine;
using CMS.Helpers;
using CMS.SiteProvider;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace KMVCHelper
{
    /// <summary>
    /// The ObjectQuery object with caching properties and handling.
    /// </summary>
    public class CacheableObjectQuery : ObjectQuery
    {
        public string CacheItemName;
        public int CacheMinutes;
        public string[] CacheDependencies;
        /// <summary>
        /// Items that are used as part of the CacheItemName, and create a unique cache name.
        /// </summary>
        public List<object> CacheItemNameParts;
        public CacheableObjectQuery() : base()
        {
            CacheItemNameParts = new List<object>();
        }
        public CacheableObjectQuery(string className) : base(className)
        {
            CacheItemNameParts = new List<object>();
        }

        /// <summary>
        /// Gets the Results (DataSet) of the results, caching if applicable
        /// </summary>
        /// <returns>The DataSet of the results</returns>
        public DataSet GetCachedResult()
        {
            if (CacheMinutes == -1)
            {
                // Check cache settings
                CacheMinutes = CacheHelper.CacheMinutes(SiteContext.CurrentSiteName);
            }
            if (string.IsNullOrWhiteSpace(CacheItemName))
            {
                CacheItemName = string.Join("|", CacheItemNameParts);
            }
            if (!EnvironmentHelper.PreviewEnabled && CacheMinutes > 0 && !string.IsNullOrWhiteSpace(CacheItemName))
            {
                return CacheHelper.Cache<DataSet>(cs =>
                {
                    if (cs.Cached)
                    {
                        cs.CacheDependency = CacheHelper.GetCacheDependency(CacheDependencies);
                    }
                    return Result;
                }, new CacheSettings(CacheMinutes, CacheItemName, "ObjectQuery_Result", CacheItemNameParts));
            }
            else
            {
                return Result;
            }
        }

        /// <summary>
        /// Gets the Typed Results of the query with caching
        /// </summary>
        /// <returns>The Typed Results.</returns>
        public InfoDataSet<BaseInfo> GetTypedResult()
        {
            if (CacheMinutes == -1)
            {
                // Check cache settings
                CacheMinutes = CacheHelper.CacheMinutes(SiteContext.CurrentSiteName);
            }
            if (string.IsNullOrWhiteSpace(CacheItemName))
            {
                CacheItemName = string.Join("|", CacheItemNameParts);
            }
            if (!EnvironmentHelper.PreviewEnabled && CacheMinutes > 0 && !string.IsNullOrWhiteSpace(CacheItemName))
            {
                return CacheHelper.Cache<InfoDataSet<BaseInfo>>(cs =>
                {
                    if (cs.Cached)
                    {
                        cs.CacheDependency = CacheHelper.GetCacheDependency(CacheDependencies);
                    }
                    return TypedResult;
                }, new CacheSettings(CacheMinutes, CacheItemName, "ObjectQuery_TypedResult", CacheItemNameParts));
            }
            else
            {
                return TypedResult;
            }
        }
    }
}