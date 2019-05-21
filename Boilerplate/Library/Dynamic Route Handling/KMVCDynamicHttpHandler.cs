using CMS.DocumentEngine;
using CMS.EventLog;
using CMS.Helpers;
using System;
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
        public void ProcessRequest(HttpContext context)
        {
            IController controller = null;
            IControllerFactory factory = ControllerBuilder.Current.GetControllerFactory();

            string DefaultController = (RequestContext.RouteData.Values.ContainsKey("controller") ? RequestContext.RouteData.Values["controller"].ToString() : "");
            string DefaultAction = (RequestContext.RouteData.Values.ContainsKey("action") ? RequestContext.RouteData.Values["action"].ToString() : "");
            string NewController = "";
            string NewAction = "Index";
            // Get the classname based on the URL
            TreeNode FoundNode = DocumentQueryHelper.GetNodeByAliasPath(context.Request.Url.AbsolutePath);
            string ClassName = FoundNode.ClassName;
            switch (ClassName.ToLower())
            {
                case "":
                    break;
                // can add your own cases to do more advanced logic if you wish
                default:
                    // Default will look towards the classname (period replaced with _) for the Controller
                    NewController = ClassName.Replace(".", "_");
                    break;
            }

            // Controller not found, use defaults
            if(string.IsNullOrWhiteSpace(NewController))
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
                // Even that failed, log and use normal HttpErrors controller
                EventLogProvider.LogException("KMVCDynamicHttpHandler", "ClassControllerNotConfigured", ex, additionalMessage: "Page found, but could not find a Controller for " + NewController + ", create one with an index view to auto handle or modify the KMVCDynamicHttpHandler");
                RequestContext.RouteData.Values["Controller"] = DefaultController;
                RequestContext.RouteData.Values["Action"] = DefaultAction;
                controller = factory.CreateController(RequestContext, DefaultController);
                controller.Execute(RequestContext);
            }
            factory.ReleaseController(controller);
        }
    }
}