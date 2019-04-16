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
    public class KMVCRouteOverPathPriorityConstraint : IRouteConstraint
    {
        public KMVCRouteOverPathPriorityConstraint()
        {
        }

        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            // Check if the controller is found and has the KMVCRouteOverPathPriority attribute.
            string ControllerName = (values.ContainsKey("controller") ? values["controller"].ToString() : "");
            return CacheHelper.Cache<bool>(cs =>
            {
                // Check if the Route that it found has the override
                IControllerFactory factory = ControllerBuilder.Current.GetControllerFactory();
                try
                {
                    var Controller = factory.CreateController(new RequestContext(httpContext, new RouteData(route, null)), ControllerName);
                    return Attribute.GetCustomAttribute(Controller.GetType(), typeof(KMVCRouteOverPathPriority)) != null;
                }
                catch (Exception)
                {
                    return false;
                }
            }, new CacheSettings(1440, "KMVCRouteOverPathPriority", ControllerName));
        }

    }

    public class KMVCPageFoundConstraint : IRouteConstraint
    {
        public bool IgnoreRootPage;

        public KMVCPageFoundConstraint(bool IgnoreRootPage = true)
        {
            this.IgnoreRootPage = IgnoreRootPage;
        }

        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            // If no document found anyway, then it will always be a match
            TreeNode FoundDoc = DocumentQueryHelper.GetNodeByAliasPath(httpContext.Request.Url.AbsolutePath);
            return (FoundDoc != null && (FoundDoc.NodeAliasPath != "/" || !IgnoreRootPage));
        }

    }

    public class KMVCRouteOverPathPriority : Attribute
    {
        public KMVCRouteOverPathPriority()
        {

        }
    }

    public class KMVCDynamicRouteHandler : IRouteHandler
    {
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return new KMVCDynamicHttpHandler(requestContext);
        }

    }

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