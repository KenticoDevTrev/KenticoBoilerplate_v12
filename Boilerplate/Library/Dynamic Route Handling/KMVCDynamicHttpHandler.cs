using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.EventLog;
using CMS.Helpers;
using Kentico.PageBuilder.Web.Mvc;
using Kentico.PageBuilder.Web.Mvc.PageTemplates;
using System;
using System.Data;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using RequestContext = System.Web.Routing.RequestContext;
namespace KMVCHelper
{
    public class KMVCDynamicHttpHandler : IHttpHandler
    {
        public RequestContext RequestContext { get; set; }
        public KMVCDynamicHttpHandler(RequestContext requestContext)
        {
            this.RequestContext = requestContext;
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Checks if the current page is using a template or not.
        /// </summary>
        /// <param name="Page">The Tree Node</param>
        /// <returns>If it has a template or not</returns>
        public static bool PageHasTemplate(TreeNode Page)
        {
            string TemplateConfiguration = Page.GetValue("DocumentPageTemplateConfiguration", "");

            // Check Temp Page builder widgets to detect a switch in template
            Guid InstanceGuid = ValidationHelper.GetGuid(URLHelper.GetQueryValue(HttpContext.Current.Request.Url.AbsoluteUri, "instance"), Guid.Empty);
            if (InstanceGuid != Guid.Empty)
            {
                DataTable Table = ConnectionHelper.ExecuteQuery(string.Format("select PageBuilderTemplateConfiguration from Temp_PageBuilderWidgets where PageBuilderWidgetsGuid = '{0}'", InstanceGuid.ToString()), null, QueryTypeEnum.SQLQuery).Tables[0];
                if (Table.Rows.Count > 0)
                {
                    TemplateConfiguration = ValidationHelper.GetString(Table.Rows[0]["PageBuilderTemplateConfiguration"], "");
                }
            }

            return !string.IsNullOrWhiteSpace(TemplateConfiguration) && !TemplateConfiguration.ToLower().Contains("\"empty.template\"");
        }

        public void ProcessRequest(HttpContext context)
        {
            IController controller = null;
            IControllerFactory factory = ControllerBuilder.Current.GetControllerFactory();

            string DefaultController = (RequestContext.RouteData.Values.ContainsKey("controller") ? RequestContext.RouteData.Values["controller"].ToString() : "");
            string DefaultAction = (RequestContext.RouteData.Values.ContainsKey("action") ? RequestContext.RouteData.Values["action"].ToString() : "");
            string NewController = "";
            string NewAction = "Index";
            // Get the classname based on the URL
            TreeNode FoundNode = DocumentQueryHelper.GetNodeByAliasPath(EnvironmentHelper.GetUrl(context.Request));
            string ClassName = FoundNode.ClassName;


            switch (ClassName.ToLower())
            {
                
                default:
                    // 
                    if(PageHasTemplate(FoundNode))
                    {
                        // Uses Page Templates, send to basic Page Template handler
                        NewController = "DynamicPageTemplate";
                        NewAction = "Index";
                    } else {
                        // Try finding a class that matches the class name
                        NewController = ClassName.Replace(".", "_");
                    }
                    break;
                // can add your own cases to do more advanced logic if you wish
                case "":
                    break;
                    
            }

            // Controller not found, use defaults
            if (string.IsNullOrWhiteSpace(NewController))
            {
                NewController = DefaultController;
                NewAction = DefaultAction;
            }

            // Setup routing with new values
            this.RequestContext.RouteData.Values["Controller"] = NewController;

            // If there is an action (2nd value), change it to the CheckNotFound, and remove ID
            if (this.RequestContext.RouteData.Values.ContainsKey("Action"))
            {
                this.RequestContext.RouteData.Values["Action"] = NewAction;
            }
            else
            {
                this.RequestContext.RouteData.Values.Add("Action", NewAction);
            }
            if (RequestContext.RouteData.Values.ContainsKey("Id"))
            {
                RequestContext.RouteData.Values.Remove("Id");
            }
            try
            {
                controller = factory.CreateController(RequestContext, NewController);
                controller.Execute(RequestContext);
            }
            catch (HttpException ex)
            {
                // Catch Controller Not implemented errors and log and go to Not Foud
                if (ex.Message.ToLower().Contains("does not implement icontroller.")) { 
                    EventLogProvider.LogException("KMVCDynamicHttpHandler", "ClassControllerNotConfigured", ex, additionalMessage: "Page found, but could not find Page Templates, nor a Controller for " + NewController + ", either create Page Templates for this class or create a controller with an index view to auto handle or modify the KMVCDynamicHttpHandler");
                    RequestContext.RouteData.Values["Controller"] = "DynamicPageTemplate";
                    RequestContext.RouteData.Values["Action"] = "NotFound";
                    controller = factory.CreateController(RequestContext, "DynamicPageTemplate");
                    controller.Execute(RequestContext);
                } else
                {
                    // This will show for any http generated exception, like view errors
                    throw new HttpException(ex.Message,ex);
                }
            }
            catch (InvalidOperationException ex)
            {
                if(ex.Message.ToLower().Contains("page template with identifier")) { 
                    // This often occurs when there is a page template assigned that is not defined
                    EventLogProvider.LogException("KMVCDynamicHttpHandler", "ClassControllerNotConfigured", ex, additionalMessage: "Page found, but contains a template that is not registered with this application.");
                    RequestContext.RouteData.Values["Controller"] = "DynamicPageTemplate";
                    RequestContext.RouteData.Values["Action"] = "UnregisteredTemplate";
                    controller = factory.CreateController(RequestContext, "DynamicPageTemplate");
                    controller.Execute(RequestContext);
                } else
                {
                    throw new InvalidOperationException(ex.Message, ex);
                }

            }
            factory.ReleaseController(controller);
        }
    }
}