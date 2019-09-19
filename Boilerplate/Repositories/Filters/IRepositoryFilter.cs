using CMS.DataEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCCaching.Kentico
{
    /// <summary>
    /// Defines a method to get a <see cref="WhereCondition"/> representing the filter configuration.
    /// </summary>
    public interface IRepositoryFilter : ICacheKey
    {
        /// <summary>
        /// Returns a filter <see cref="WhereCondition"/>.
        /// </summary>
        WhereCondition GetWhereCondition();
    }
}