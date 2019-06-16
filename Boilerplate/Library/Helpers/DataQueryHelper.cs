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
    public class DataQueryHelper
    {
        /// <summary>
        /// Mimics the Repeater with Custom Query, including caching
        /// </summary>
        /// <param name="QueryName">The Query Name</param>
        /// <param name="OrderBy">Order By</param>
        /// <param name="SelectTopN">TopN</param>
        /// <param name="WhereCondition">Where Condition</param>
        /// <param name="Columns">Columns</param>
        /// <param name="QueryParameters">Query Parameters to pass to query.</param>
        /// <param name="CacheItemName">Unique identifier for the cache, REQUIRED for caching.</param>
        /// <param name="CacheMinutes">Optional Cache Minutes, will use site's default cache minutes if not provided.</param>
        /// <param name="CacheDependencies">Cache Dependencies</param>
        /// <returns></returns>
        public static DataSet ExecuteQuery(
            string QueryName,
            string OrderBy = null,
            int SelectTopN = -1,
            string WhereCondition = null,
            string Columns = null,
            QueryDataParameters QueryParameters = null,
            string CacheItemName = null,
            int CacheMinutes = -1,
            string[] CacheDependencies = null)
        {

            List<object> CacheItemNameParts = new List<object>();

            if (CacheMinutes == -1)
            {
                // Check cache settings
                CacheMinutes = CacheHelper.CacheMinutes(SiteContext.CurrentSiteName);
            }

            if (CacheMinutes > 0 && !string.IsNullOrWhiteSpace(CacheItemName))
            {
                // Fill up the CacheItemNameParts
                if (QueryParameters != null)
                {
                    foreach(DataParameter QueryParam in QueryParameters)
                    {
                        CacheItemNameParts.Add(string.Format("{0}|{1}", QueryParam.Name, QueryParam.Value));
                    }
                }
                if(!string.IsNullOrWhiteSpace(WhereCondition))
                {
                    CacheItemNameParts.Add(WhereCondition);
                }
                if(!string.IsNullOrWhiteSpace(OrderBy))
                {
                    CacheItemNameParts.Add(OrderBy);
                }
                if(!string.IsNullOrWhiteSpace(Columns))
                {
                    CacheItemNameParts.Add(Columns);
                }
                if(SelectTopN > -1)
                {
                    CacheItemNameParts.Add(SelectTopN);
                }
                return CacheHelper.Cache<DataSet>(cs =>
                {
                    if (cs.Cached)
                    {
                        cs.CacheDependency = CacheHelper.GetCacheDependency(CacheDependencies);
                    }
                    return ConnectionHelper.ExecuteQuery(QueryName, QueryParameters, WhereCondition, OrderBy, SelectTopN, Columns);
                }, new CacheSettings(CacheMinutes, CacheItemName, "Result", QueryName, CacheItemNameParts));
            }
            else
            {
                return ConnectionHelper.ExecuteQuery(QueryName, QueryParameters, WhereCondition, OrderBy, SelectTopN, Columns);
            }

        }

        /// <summary>
        /// ObjectQuery class with Caching taken into affect
        /// </summary>
        /// <param name="ClassName">The ClassName</param>
        /// <param name="OrderBy">Order By</param>
        /// <param name="SelectTopN">Top N</param>
        /// <param name="WhereCondition">Where Condition</param>
        /// <param name="Columns">Columns</param>
        /// <param name="CacheItemName">The Cache Item name, required if you wish to cache.</param>
        /// <param name="CacheMinutes">Cache Minutes, defaults to the site's cache minutes</param>
        /// <param name="CacheDependencies">Any cache dependencies</param>
        /// <returns>The CacheableObjectQuery which you can further modify and then call the CachedResults</returns>
        public static CacheableObjectQuery ObjectQuery(
            string ClassName,
            string OrderBy = null,
            int SelectTopN = -1,
            string WhereCondition = null,
            string Columns = null,
            string CacheItemName = null,
            int CacheMinutes = -1,
            string[] CacheDependencies = null
            )
        {
            CacheableObjectQuery ObjectQuery = new CacheableObjectQuery(ClassName);
            if(!string.IsNullOrWhiteSpace(OrderBy))
            {
                ObjectQuery.OrderBy(OrderBy);
                ObjectQuery.CacheItemNameParts.Add(OrderBy);
            }
            if(SelectTopN > -1)
            {
                ObjectQuery.TopN(SelectTopN);
                ObjectQuery.CacheItemNameParts.Add(SelectTopN);
            }
            if(!string.IsNullOrWhiteSpace(WhereCondition))
            {
                ObjectQuery.Where(WhereCondition);
                ObjectQuery.CacheItemNameParts.Add(WhereCondition);
            }
            if(!string.IsNullOrWhiteSpace(Columns))
            {
                ObjectQuery.Columns(Columns);
                ObjectQuery.CacheItemNameParts.Add(Columns);
            }
            ObjectQuery.CacheMinutes = CacheMinutes;
            if (!string.IsNullOrWhiteSpace(CacheItemName))
            {
                ObjectQuery.CacheItemNameParts.Add(CacheItemName);
            }
            return ObjectQuery;
        }
    }

}