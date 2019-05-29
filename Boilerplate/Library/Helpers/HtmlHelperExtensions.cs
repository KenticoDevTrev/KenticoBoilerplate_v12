using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

using CMS.DocumentEngine;
using CMS.Helpers;
using CMS.Membership;

using Kentico.Content.Web.Mvc;
using Kentico.Web.Mvc;

namespace KMVCHelper
{
    /// <summary>
    /// Extension methods for <see cref="HtmlHelper{TModel}"/> class.
    /// </summary>
    public static class HtmlHelperExtensions
    {
        /// <summary>
        /// Writes an End Div Tag
        /// </summary>
        /// <param name="htmlHelper">The Html Helper</param>
        /// <returns>The end Tag of the Div</returns>
        public static MvcHtmlString EndDiv(this HtmlHelper htmlHelper)
        {
            return MvcHtmlString.Create("</div>");
        }

        /// <summary>
        /// Creates a Starting Div
        /// </summary>
        /// <param name="ID">The ID</param>
        /// <param name="Name">The Name</param>
        /// <param name="Class">The CSS Class</param>
        /// <param name="HtmlAttributes">Additional Attributes</param>
        /// <returns>The Start Div</returns>
        public static MvcHtmlString BeginDiv(this HtmlHelper htmlHelper, string ID = null, string Name = null, string Class = null, IDictionary<string, object> HtmlAttributes = null)
        {
            return Div(htmlHelper, true, ID, Name, Class, HtmlAttributes);
        }

      

        /// <summary>
        /// Generates a Div tag
        /// </summary>
        /// <param name="ID">The ID</param>
        /// <param name="Name">The Name</param>
        /// <param name="Class">The CSS Class</param>
        /// <param name="HtmlAttributes">Additional Attributes</param>
        /// <param name="StartTagOnly">If you wish to only generate the open tag vs. the self closed tag.</param>
        /// <returns>The div tag</returns>
        public static MvcHtmlString Div(this HtmlHelper htmlHelper, bool StartTagOnly = false, string ID = null, string Name = null, string Class = null, IDictionary<string, object> HtmlAttributes = null)
        {
            var DivTag = new TagBuilder("div");
            if(!string.IsNullOrWhiteSpace(ID))
            {
                DivTag.Attributes.Add("id", ID);
            }
            if(!string.IsNullOrWhiteSpace(Name))
            {
                DivTag.Attributes.Add("name", Name);
            }
            if (!string.IsNullOrWhiteSpace(Class))
            {
                DivTag.Attributes.Add("class", Class);
            }
            if (HtmlAttributes != null)
            {
                foreach (string key in HtmlAttributes.Keys)
                {
                    DivTag.Attributes.Add(key, (HtmlAttributes[key] != null ? HtmlAttributes[key].ToString() : null));
                }
            }

            return MvcHtmlString.Create(DivTag.ToString((StartTagOnly ? TagRenderMode.StartTag : TagRenderMode.SelfClosing)));
        }


        /// <summary>
        /// Returns an HTML input element with a label and validation fields for each property in the object that is represented by the <see cref="Expression"/> expression.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="html">The HTML helper instance that this method extends.</param>
        /// <param name="expression">An expression that identifies the object that contains the displayed properties.</param>
        /// <param name="explanationText">An explanation text describing usage of the rendered field.</param>
        /// <param name="disabled">Indicates that field has to be disabled.</param>
        public static MvcHtmlString ValidatedEditorFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, string explanationText = "", bool disabled = false)
        {
            var label = html.LabelFor(expression).ToString();
            var additionalViewData = disabled ? new { htmlAttributes = new { disabled = "disabled" } } : null;
            var editor = html.EditorFor(expression, additionalViewData).ToString();
            var message = html.ValidationMessageFor(expression).ToString();
            var explanationTextHtml = "";

            if (!string.IsNullOrEmpty(explanationText))
            {
                explanationTextHtml = "<div class=\"explanation-text\">" + explanationText + "</div>";
            }

            var generatedHtml = string.Format(@"
<div class=""form-group"">
    <div class=""form-group-label"">{0}</div>
    <div class=""form-group-input"">{1}
       {2}
    </div>
    <div class=""message message-error"">{3}</div>
</div>", label, editor, explanationTextHtml, message);

            return MvcHtmlString.Create(generatedHtml);
        }


        /// <summary>
        /// Generates an A tag with a "mailto" link.
        /// </summary>
        /// <param name="htmlHelper">HTML helper.</param>
        /// <param name="email">Email address.</param>
        public static MvcHtmlString MailTo(this HtmlHelper htmlHelper, string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return MvcHtmlString.Empty;
            }

            var link = string.Format("<a href=\"mailto:{0}\">{1}</a>", HTMLHelper.EncodeForHtmlAttribute(email), HTMLHelper.HTMLEncode(email));

            return MvcHtmlString.Create(link);
        }


        /// <summary>
        /// Generates an IMG tag for an attachment.
        /// </summary>
        /// <param name="htmlHelper">HTML helper.</param>
        /// <param name="attachment">Attachment object.</param>
        /// <param name="title">Title.</param>
        /// <param name="cssClassName">CSS class.</param>
        /// <param name="constraint">Size constraint.</param>
        public static MvcHtmlString AttachmentImage(this HtmlHelper htmlHelper, DocumentAttachment attachment, string title = "", string cssClassName = "", SizeConstraint? constraint = null)
        {
            if (attachment == null)
            {
                return MvcHtmlString.Empty;
            }

            return Image(htmlHelper, attachment.GetPath(), title, cssClassName, constraint);
        }


        /// <summary>
        /// Generates an IMG tag for an image file.
        /// </summary>
        /// <param name="htmlHelper">HTML helper.</param>
        /// <param name="path">The virtual path of the image.</param>
        /// <param name="title">Title.</param>
        /// <param name="cssClassName">CSS class.</param>
        /// <param name="constraint">Size constraint.</param>
        public static MvcHtmlString Image(this HtmlHelper htmlHelper, string path, string title = "", string cssClassName = "", SizeConstraint? constraint = null)
        {
            if (string.IsNullOrEmpty(path))
            {
                return MvcHtmlString.Empty;
            }

            var urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);
            var image = new TagBuilder("img");
            image.MergeAttribute("src", urlHelper.Kentico().ImageUrl(path, constraint.GetValueOrDefault(SizeConstraint.Empty)));
            image.AddCssClass(cssClassName);
            image.MergeAttribute("alt", title);
            image.MergeAttribute("title", title);

            return MvcHtmlString.Create(image.ToString(TagRenderMode.SelfClosing));
        }


        /// <summary>
        /// Generates a link for an actual page with the specified culture.
        /// </summary>
        /// <param name="htmlHelper">HTML helper.</param>
        /// <param name="linkText">Displayed text.</param>
        /// <param name="cultureName">Name of the new culture.</param>
        /// <returns>RouteLink for the given culture.</returns>
        public static MvcHtmlString CultureLink(this HtmlHelper htmlHelper, string linkText, string cultureName)
        {
            var queryParams = htmlHelper.ViewContext.HttpContext.Request.Unvalidated.QueryString;
            var originalRouteValues = htmlHelper.ViewContext.RouteData.Values;

            // Create a link for the current culture (the URL stays as it is)
            if ((string)originalRouteValues["culture"] == cultureName)
            {
                return htmlHelper.HtmlLink(htmlHelper.ViewContext.HttpContext.Request.RawUrl, linkText);
            }

            // Clone the original route information
            // All route values are reflected in the URL (either in the URL path or in the query string parameters)
            // therefore try to filter-out all route values that are likely used only internally by Kentico and should not make it into the URL.
            var newRouteValues = new RouteValueDictionary();
            var routeValues = originalRouteValues.Where(r => !r.Key.StartsWith("Kentico", StringComparison.OrdinalIgnoreCase));
            foreach (var routeValue in routeValues)
            {
                newRouteValues.Add(routeValue.Key, routeValue.Value);
            }

            // Add query parameters (e.g. when performing a search)
            foreach (string key in queryParams)
            {
                // Remove the key from the collection before adding a new value to prevent throwing an exception
                if (!String.IsNullOrEmpty(key))
                {
                    newRouteValues.Remove(key);
                    newRouteValues.Add(key, queryParams[key]);
                }
            }

            // Modify the original culture to the new one
            newRouteValues["culture"] = cultureName;

            return htmlHelper.RouteLink(linkText, newRouteValues);
        }


        /// <summary>
        /// Generates an A tag with a link to the specified URL.
        /// </summary>
        /// <param name="htmlHelper">HTML helper.</param>
        /// <param name="url">Link URL.</param>
        /// <param name="linkText">Displayed text.</param>
        /// <param name="htmlAttributes">Additional tag attributes.</param>
        public static MvcHtmlString HtmlLink(this HtmlHelper htmlHelper, string url, string linkText, object htmlAttributes = null)
        {
            var link = new TagBuilder("a")
            {
                InnerHtml = linkText
            };
            link.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            link.MergeAttribute("href", url);

            return MvcHtmlString.Create(link.ToString());
        }


        /// <summary>
        /// Returns localization for given resource string key in a preferred UI culture of current user.
        /// </summary>
        /// <param name="htmlHelper">HTML helper.</param>
        /// <param name="key">Resource string key.</param>
        /// <remarks>Returned string is not HTML encoded.</remarks>
        public static IHtmlString GetUIString(this HtmlHelper htmlHelper, string key)
        {
            return htmlHelper.Raw(ResHelper.GetString(key, MembershipContext.AuthenticatedUser.PreferredUICultureCode));
        }
    }
}