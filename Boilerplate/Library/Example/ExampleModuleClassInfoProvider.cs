using System;
using System.Data;

using CMS.Base;
using CMS.DataEngine;
using CMS.Helpers;

namespace MVCCaching.Kentico.Example
{
    /// <summary>
    /// Class providing <see cref="ExampleModuleClassInfo"/> management.
    /// </summary>
    public partial class ExampleModuleClassInfoProvider : AbstractInfoProvider<ExampleModuleClassInfo, ExampleModuleClassInfoProvider>
    {
        /// <summary>
        /// Creates an instance of <see cref="ExampleModuleClassInfoProvider"/>.
        /// </summary>
        public ExampleModuleClassInfoProvider()
            : base(ExampleModuleClassInfo.TYPEINFO)
        {
        }


        /// <summary>
        /// Returns a query for all the <see cref="ExampleModuleClassInfo"/> objects.
        /// </summary>
        public static ObjectQuery<ExampleModuleClassInfo> GetExampleModuleClasses()
        {
            return ProviderObject.GetObjectQuery();
        }


        /// <summary>
        /// Returns <see cref="ExampleModuleClassInfo"/> with specified ID.
        /// </summary>
        /// <param name="id"><see cref="ExampleModuleClassInfo"/> ID.</param>
        public static ExampleModuleClassInfo GetExampleModuleClassInfo(int id)
        {
            return ProviderObject.GetInfoById(id);
        }


        /// <summary>
        /// Returns <see cref="ExampleModuleClassInfo"/> with specified name.
        /// </summary>
        /// <param name="name"><see cref="ExampleModuleClassInfo"/> name.</param>
        public static ExampleModuleClassInfo GetExampleModuleClassInfo(string name)
        {
            return ProviderObject.GetInfoByCodeName(name);
        }


        /// <summary>
        /// Sets (updates or inserts) specified <see cref="ExampleModuleClassInfo"/>.
        /// </summary>
        /// <param name="infoObj"><see cref="ExampleModuleClassInfo"/> to be set.</param>
        public static void SetExampleModuleClassInfo(ExampleModuleClassInfo infoObj)
        {
            ProviderObject.SetInfo(infoObj);
        }


        /// <summary>
        /// Deletes specified <see cref="ExampleModuleClassInfo"/>.
        /// </summary>
        /// <param name="infoObj"><see cref="ExampleModuleClassInfo"/> to be deleted.</param>
        public static void DeleteExampleModuleClassInfo(ExampleModuleClassInfo infoObj)
        {
            ProviderObject.DeleteInfo(infoObj);
        }


        /// <summary>
        /// Deletes <see cref="ExampleModuleClassInfo"/> with specified ID.
        /// </summary>
        /// <param name="id"><see cref="ExampleModuleClassInfo"/> ID.</param>
        public static void DeleteExampleModuleClassInfo(int id)
        {
            ExampleModuleClassInfo infoObj = GetExampleModuleClassInfo(id);
            DeleteExampleModuleClassInfo(infoObj);
        }
    }
}