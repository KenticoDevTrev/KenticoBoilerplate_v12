using Kentico.Forms.Web.Mvc;
using Kentico.FormComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CMS.Helpers;

[assembly: RegisterFormComponent(TinyMCEInputComponent.IDENTIFIER, typeof(TinyMCEInputComponent), "TinyMCE WYSIWYG Editor", Description = "HTML Editor", IconClass = "icon-paragraph")]
namespace Kentico.FormComponents
{
    public class TinyMCEInputComponent : FormComponent<TinyMCEInputComponentProperties, string>
    {

        public const string IDENTIFIER = "TinyMCEInputComponent";
        // Disables automatic server-side evaluation for the component
        public override bool CustomAutopostHandling => true;

        [BindableProperty]
        public string HtmlContent { get; set; } = "";

        public override string GetValue()
        {
            return HtmlContent;
        }

        public override void SetValue(string value)
        {
            HtmlContent = ValidationHelper.GetString(value, "");
        }

        /// <summary>
        /// Indicates if the formatting is enabled for the editor.
        /// </summary>
        public bool EnableFormatting { get; set; } = true;

        private string _Tools;

        public string Tools
        {
            get
            {
                if (!string.IsNullOrEmpty(_Tools) && !_Tools.EndsWith("tinyMceAttributionButton"))
                {
                    return _Tools + " | tinyMceAttributionButton";
                }

                return _Tools ?? "undo redo | formatselect | bold italic backcolor | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | removeformat | help fullscreen | tinyMceAttributionButton";
            }
            set
            {


                _Tools = value;
            }
        }

        public string Plugins { get; set; } = "advlist autolink lists link image print preview anchor searchreplace visualblocks code fullscreen insertdatetime media table paste help wordcount";
    }
}