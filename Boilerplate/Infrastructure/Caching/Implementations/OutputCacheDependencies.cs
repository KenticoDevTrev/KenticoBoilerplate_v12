using System;
using System.Collections.Generic;
using System.Web;
using CMS.AspNet.Platform.Cache.Extension;
using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.Helpers;
using CMS.SiteProvider;

namespace MVCCaching.Kentico
{
    /// <summary>
    /// Creates a minimum set of ASP.NET output cache dependencies for views that contain data from pages or info objects.
    /// </summary>
    public sealed class OutputCacheDependencies : IOutputCacheDependencies
    {
        private readonly HttpResponseBase mResponse;
        private readonly IContentItemMetadataProvider mContentItemMetadataProvider;
        private readonly bool mCacheEnabled;
        private readonly HashSet<string> mDependencyCacheKeys = new HashSet<string>();


        /// <summary>
        /// Initializes a new instance of the <see cref="OutputCacheDependencies"/> class.
        /// </summary>
        /// <param name="response">HTTP response that will be used to create output cache dependencies.</param>
        /// <param name="contentItemMetadataProvider">object that provides information about pages and info objects using their runtime type.</param>
        /// <param name="cacheEnabled">Indicates whether caching is enabled.</param>
        public OutputCacheDependencies(HttpResponseBase response, IContentItemMetadataProvider contentItemMetadataProvider, bool cacheEnabled)
        {
            mResponse = response;
            mContentItemMetadataProvider = contentItemMetadataProvider;
            mCacheEnabled = cacheEnabled;
        }


        /// <summary>
        /// Adds a minimum set of ASP.NET output cache dependencies for a view that contains data from pages of the specified runtime type.
        /// When any page of the specified runtime type is created, updated or deleted, the corresponding output cache item is invalidated.
        /// </summary>
        /// <typeparam name="T">Runtime type that represents pages, i.e. it is derived from the <see cref="TreeNode"/> class.</typeparam>
        public void AddDependencyOnPages<T>() where T : TreeNode, new()
        {
            if (!mCacheEnabled)
            {
                return;
            }

            var className = mContentItemMetadataProvider.GetClassNameFromPageRuntimeType<T>();
            var dependencyCacheKey = String.Format("nodes|{0}|{1}|all", SiteContext.CurrentSiteName.ToLowerInvariant(), className);

            AddCacheItemDependency(dependencyCacheKey);
            AddCacheItemDependency("cms.adhocrelationship|all");
            AddCacheItemDependency("cms.relationship|all");
        }


        /// <summary>
        /// Adds a minimum set of ASP.NET output cache dependencies for a view that contains data from info objects of the specified runtime type.
        /// When any info object of the specified runtime type is created, updated or deleted, the corresponding output cache item is invalidated.
        /// </summary>
        /// <typeparam name="T">Runtime type that represents info objects, i.e. it is derived from the <see cref="AbstractInfo{TInfo}"/> class.</typeparam>
        public void AddDependencyOnInfoObjects<T>() where T : AbstractInfo<T>, new()
        {
            if (!mCacheEnabled)
            {
                return;
            }

            var objectType = mContentItemMetadataProvider.GetObjectTypeFromInfoObjectRuntimeType<T>();
            var dependencyCacheKey = String.Format("{0}|all", objectType);
            AddCacheItemDependency(dependencyCacheKey);
        }


        /// <summary>
        /// Adds a minimum set of ASP.NET output cache dependencies for a view that contains data from info object of the specified runtime type.
        /// When info object of the specified runtime type is created, updated or deleted, the corresponding output cache item is invalidated.
        /// </summary>
        /// <param name="infoGuid">Info object guid used for dependency cache key.</param>
        /// <typeparam name="T">Runtime type that represents info objects, i.e. it is derived from the <see cref="AbstractInfo{TInfo}"/> class.</typeparam>
        public void AddDependencyOnInfoObject<T>(Guid infoGuid) where T : AbstractInfo<T>, new()
        {
            if (!mCacheEnabled || infoGuid == Guid.Empty)
            {
                return;
            }

            var objectType = mContentItemMetadataProvider.GetObjectTypeFromInfoObjectRuntimeType<T>();
            var dependencyCacheKey = $"{objectType}|byguid|{infoGuid}";
            AddCacheItemDependency(dependencyCacheKey);
        }


        /// <summary>
        /// Adds a minimum set of ASP.NET output cache dependencies for a view that contains data from page attachment.
        /// When specified attachment is created, updated or deleted, the corresponding output cache item is invalidated.
        /// </summary>
        /// <param name="attachmentGuid">Attachment guid used for dependency cache key.</param>
        public void AddDependencyOnPageAttachmnent(Guid attachmentGuid)
        {
            if (!mCacheEnabled || attachmentGuid == Guid.Empty)
            {
                return;
            }

            var dependencyCacheKey = $"attachment|{attachmentGuid}";
            AddCacheItemDependency(dependencyCacheKey);
        }


        /// <summary>
        /// Adds a minimum set of ASP.NET output cache dependencies for a view that contains data from page of the specified runtime type.
        /// When specified page of the specified runtime type is created, updated or deleted, the corresponding output cache item is invalidated.
        /// </summary>
        /// <param name="documentId">Document id used for dependency cache key.</param>
        /// <typeparam name="T">Runtime type that represents pages, i.e. it is derived from the <see cref="TreeNode"/> class.</typeparam>
        public void AddDependencyOnPage<T>(int documentId) where T : TreeNode, new()
        {
            if (!mCacheEnabled || documentId == 0)
            {
                return;
            }

            var dependencyCacheKey = $"documentid|{documentId}";

            AddCacheItemDependency(dependencyCacheKey);
        }

        /// <summary>
        /// Adds the custom Cache Dependency for a view.
        /// </summary>
        /// <param name="dependencyCacheKey">The Kentico Cache Dependency Key</param>
        public void AddCacheItemDependency(string dependencyCacheKey)
        {
            if (!mCacheEnabled)
            {
                return;
            }
            // Ensure to lower variant
            dependencyCacheKey = dependencyCacheKey.ToLowerInvariant();

            if (!mDependencyCacheKeys.Contains(dependencyCacheKey))
            {
                mDependencyCacheKeys.Add(dependencyCacheKey);
                CacheHelper.EnsureDummyKey(dependencyCacheKey);
                mResponse.AddCacheDependency(new CMSCacheDependency(null, new string[] { dependencyCacheKey }, DateTime.Now).CreateCacheDependency());
            }
        }

        /// <summary>
        /// Adds the custom Cache Dependencies for a view.
        /// </summary>
        /// <param name="dependencyCacheKey">The Kentico Cache Dependency Keys</param>
        public void AddCacheItemDependencies(IEnumerable<string> dependencyCacheKeys)
        {
            if (!mCacheEnabled)
            {
                return;
            }

            foreach (string dependencyCacheKey in dependencyCacheKeys)
            {
                AddCacheItemDependency(dependencyCacheKey);
            }
        }
    }
}