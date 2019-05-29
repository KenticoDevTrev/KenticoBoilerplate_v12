using CMS.DataEngine;
using Kentico.Forms.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kentico.FormComponents
{
    public class TinyMCEInputComponentProperties : FormComponentProperties<string>
    {
        [DefaultValueEditingComponent("TinyMCEInputComponent", DefaultValue = "")]
        public override string DefaultValue
        {
            get;
            set;
        }

        // Initializes a new instance of the RgbInputComponentProperties class and configures the underlying database field
        public TinyMCEInputComponentProperties()
            : base(FieldDataType.Text)
        {
        }
    }
}