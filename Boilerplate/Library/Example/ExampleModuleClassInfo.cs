using System;
using System.Data;
using System.Runtime.Serialization;

using CMS;
using CMS.DataEngine;
using CMS.Helpers;
using MVCCaching.Kentico.Example;

[assembly: RegisterObjectType(typeof(ExampleModuleClassInfo), ExampleModuleClassInfo.OBJECT_TYPE)]

namespace MVCCaching.Kentico.Example
{
    /// <summary>
    /// Data container class for <see cref="ExampleModuleClassInfo"/>.
    /// </summary>
    [Serializable]
    public partial class ExampleModuleClassInfo : AbstractInfo<ExampleModuleClassInfo>
    {
        /// <summary>
        /// Object type.
        /// </summary>
        public const string OBJECT_TYPE = "example.examplemoduleclass";


        /// <summary>
        /// Type information.
        /// </summary>
#warning "You will need to configure the type info."
        public static readonly ObjectTypeInfo TYPEINFO = new ObjectTypeInfo(typeof(ExampleModuleClassInfoProvider), OBJECT_TYPE, "Example.ExampleModuleClass", "ExampleModuleClassID", "ExampleModuleClassLastModified", "ExampleModuleClassGuid", null, "ExampleModuleClassName", null, null, null, null)
        {
            ModuleName = "Boilerplate",
            TouchCacheDependencies = true,
        };


        /// <summary>
        /// Example module class ID.
        /// </summary>
        [DatabaseField]
        public virtual int ExampleModuleClassID
        {
            get
            {
                return ValidationHelper.GetInteger(GetValue("ExampleModuleClassID"), 0);
            }
            set
            {
                SetValue("ExampleModuleClassID", value);
            }
        }


        /// <summary>
        /// Example module class name.
        /// </summary>
        [DatabaseField]
        public virtual string ExampleModuleClassName
        {
            get
            {
                return ValidationHelper.GetString(GetValue("ExampleModuleClassName"), String.Empty);
            }
            set
            {
                SetValue("ExampleModuleClassName", value);
            }
        }


        /// <summary>
        /// Example module class guid.
        /// </summary>
        [DatabaseField]
        public virtual Guid ExampleModuleClassGuid
        {
            get
            {
                return ValidationHelper.GetGuid(GetValue("ExampleModuleClassGuid"), Guid.Empty);
            }
            set
            {
                SetValue("ExampleModuleClassGuid", value);
            }
        }


        /// <summary>
        /// Example module class last modified.
        /// </summary>
        [DatabaseField]
        public virtual DateTime ExampleModuleClassLastModified
        {
            get
            {
                return ValidationHelper.GetDateTime(GetValue("ExampleModuleClassLastModified"), DateTimeHelper.ZERO_TIME);
            }
            set
            {
                SetValue("ExampleModuleClassLastModified", value);
            }
        }


        /// <summary>
        /// Deletes the object using appropriate provider.
        /// </summary>
        protected override void DeleteObject()
        {
            ExampleModuleClassInfoProvider.DeleteExampleModuleClassInfo(this);
        }


        /// <summary>
        /// Updates the object using appropriate provider.
        /// </summary>
        protected override void SetObject()
        {
            ExampleModuleClassInfoProvider.SetExampleModuleClassInfo(this);
        }


        /// <summary>
        /// Constructor for de-serialization.
        /// </summary>
        /// <param name="info">Serialization info.</param>
        /// <param name="context">Streaming context.</param>
        protected ExampleModuleClassInfo(SerializationInfo info, StreamingContext context)
            : base(info, context, TYPEINFO)
        {
        }


        /// <summary>
        /// Creates an empty instance of the <see cref="ExampleModuleClassInfo"/> class.
        /// </summary>
        public ExampleModuleClassInfo()
            : base(TYPEINFO)
        {
        }


        /// <summary>
        /// Creates a new instances of the <see cref="ExampleModuleClassInfo"/> class from the given <see cref="DataRow"/>.
        /// </summary>
        /// <param name="dr">DataRow with the object data.</param>
        public ExampleModuleClassInfo(DataRow dr)
            : base(TYPEINFO, dr)
        {
        }
    }
}