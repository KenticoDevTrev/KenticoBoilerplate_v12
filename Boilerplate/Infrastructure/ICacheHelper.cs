using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kentico.Caching
{
    public interface ICacheHelper
    {
        /// <summary>
        /// Either Retrieves the object if cached, or executes and caches the method given the KeyName, with the Dependencies for the given Cache Duration
        /// </summary>
        /// <typeparam name="T">The Return Object type</typeparam>
        /// <param name="Func">The Function</param>
        /// <param name="KeyName">The Cache Key Name</param>
        /// <param name="Dependencies">Any Cache Dependencies</param>
        /// <param name="CacheDuration">The Duration of the Cache</param>
        /// <returns></returns>
        T Cache<T>(Func<T> Func, string KeyName, IEnumerable<string> Dependencies, TimeSpan CacheDuration);
        
        /// <summary>
        /// Clears any caches with the given Name
        /// </summary>
        /// <param name="KeyName">The Cache Key</param>
        void TouchKey(string KeyName);

        /// <summary>
        /// Clears any caches with any of the given Names
        /// </summary>
        /// <param name="KeyNames">The Cache Keys</param>
        void TouchKeys(IEnumerable<string> KeyNames);

        /// <summary>
        /// Gets the Cache Duration.
        /// </summary>
        /// <param name="ObjectIdentifier">An identifier that can be used if the Cache Duration should be different.</param>
        /// <returns>The TimeSpan of the Cache Duration</returns>
        TimeSpan CacheDuration(string ObjectIdentifier = "");
    }
}