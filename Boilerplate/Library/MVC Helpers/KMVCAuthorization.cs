using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.Helpers;
using CMS.Membership;
using CMS.SiteProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KMVCHelper
{
    public class KMVCAuthorization : AuthorizeAttribute
    {
        /// <summary>
        /// True by default, if no other attributes specified and this is true, then will authorize any logged in user.  Set to false to  
        /// </summary>
        public bool UserAuthenticationRequired { get; set; } = true;

        /// <summary>
        /// Comma, semi-color or pipe delimited list of ResourceName+PermissionName, such as CMS.Blog.Modify|My_Module.MyCustomPermission
        /// </summary>
        public string ResourceAndPermissionNames { get; set; }
        /// <summary>
        /// Set to true to leverage Kentico's Page Security, must be able to find the node for this check to run
        /// </summary>
        public bool CheckPageACL { get; set; } = false;
        /// <summary>
        /// The Node Permission this will check when it does an ACL check.  Default is Read
        /// </summary>
        public NodePermissionsEnum NodePermissionToCheck { get; set; } = NodePermissionsEnum.Read;
        /// <summary>
        /// Custom redirect path, useful if you want to direct users to a specific unauthorized page or perhaps a JsonResult action for AJAX calls.
        /// </summary>
        public string CustomUnauthorizedRedirect { get; set; }
        /// <summary>
        /// True by default, this will cache authentication requests using Kentico's CacheHelper.
        /// </summary>
        public bool CacheAuthenticationResults { get; set; } = true;

        /// <summary>
        /// Can override this if you need to implement custom logic, such as a custom route.  httpContext.Request.RequestContext.RouteData.Values is often used to grab route data.
        /// </summary>
        /// <param name="httpContext">The HttpContext of the request</param>
        /// <returns>The Tree Node for this request, null acceptable.</returns>
        public virtual TreeNode GetTreeNode(HttpContextBase httpContext)
        {
            string Path = EnvironmentHelper.GetUrl(HttpContext.Current.Request);
            // return GetNodeByAliasPath(Path);

            // This is the same logic in my DocumentQueryHelper.GetNodeByAliasPath(), added here so it does not depend on my classes.
            string ClassName = null;
            string CultureCode = null;
            return CacheHelper.Cache<TreeNode>(cs =>
            {
                List<string> CacheDependencies = new List<string>();
                TreeNode FoundNode = DocumentQueryHelper.RepeaterQuery(Path: Path, ClassNames: ClassName, CultureCode: CultureCode).GetTypedResult().Items.FirstOrDefault();
                if (FoundNode == null)
                {
                    // Check Url Aliases
                    var FoundNodeByAlias = DocumentAliasInfoProvider.GetDocumentAliasesWithNodesDataQuery().WhereEquals("AliasUrlPath", Path).Or().Where(string.Format("'{0}' like AliasWildCardRule", SqlHelper.EscapeQuotes(Path))).FirstOrDefault();
                    if (FoundNodeByAlias != null && FoundNodeByAlias.AliasNodeID > 0)
                    {
                        CacheDependencies.Add("cms.documentalias|all");
                        CacheDependencies.Add(string.Format("node|{0}|{1}", SiteContext.CurrentSiteName, Path));
                        FoundNode = DocumentQueryHelper.RepeaterQuery(NodeID: FoundNodeByAlias.AliasNodeID, ClassNames: ClassName, CultureCode: (!string.IsNullOrWhiteSpace(FoundNodeByAlias.AliasCulture) ? FoundNodeByAlias.AliasCulture : CultureCode)).GetTypedResult().Items.FirstOrDefault();
                    }
                }
                if (FoundNode != null)
                {
                    CacheDependencies.Add("documentid|" + FoundNode.DocumentID);
                }
                if (cs.Cached)
                {
                    cs.CacheDependency = CacheHelper.GetCacheDependency(CacheDependencies.ToArray());
                }
                return FoundNode;
            }, new CacheSettings(CacheHelper.CacheMinutes(SiteContext.CurrentSiteName), "GetNodeByAliasPath", Path, ClassName, CultureCode));
        }

        /// <summary>
        /// Can override this to implement your own custom logic to get the UserInfo of the current user.  Use httpContext.User
        /// </summary>
        /// <param name="httpContext">The HttpContext of the request</param>
        /// <returns>The UserInfo, should return the Public user if they are not logged in.</returns>
        public virtual UserInfo GetCurrentUser(HttpContextBase httpContext)
        {
            // return EnvironmentHelper.AuthenticatedUser(httpContext.User);
            // This logic is the same as my EnvironmentHelper.AuthenticatedUser(httpContext.User), added so does not depend on my classes.
            string Username = DataHelper.GetNotEmpty((httpContext.User != null && httpContext.User.Identity != null ? httpContext.User.Identity.Name : "public"), "public");
            return CacheHelper.Cache<UserInfo>(cs =>
            {
                UserInfo UserObj = UserInfoProvider.GetUserInfo(Username);
                if (UserObj == null)
                {
                    UserObj = UserInfoProvider.GetUserInfo("public");
                }
                if (cs.Cached)
                {
                    cs.CacheDependency = CacheHelper.GetCacheDependency("cms.user|byid|" + UserObj.UserID);
                }
                return UserObj;
            }, new CacheSettings(CacheHelper.CacheMinutes(SiteContext.CurrentSiteName), "AuthenticatedUser", Username));
        }

        /// <summary>
        /// Checks Roles, Users, Resource Names, and Page ACL depending on configuration
        /// </summary>
        /// <param name="httpContext">The Route Context</param>
        /// <returns>If the request is authorized.</returns>
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var CurrentUser = GetCurrentUser(httpContext);
            TreeNode FoundPage = GetTreeNode(httpContext);

            return CacheHelper.Cache<bool>(cs =>
            {
                List<string> CacheDependencies = new List<string>();
                bool Authorized = false;

                // Will remain true only if no other higher priority authorization items were specified
                bool OnlyAuthenticatedCheck = true;

                // Roles
                if (!Authorized && !string.IsNullOrWhiteSpace(Roles))
                {
                    OnlyAuthenticatedCheck = false;
                    CacheDependencies.Add("cms.role|all");
                    CacheDependencies.Add("cms.userrole|all");
                    CacheDependencies.Add("cms.membershiprole|all");
                    CacheDependencies.Add("cms.membershipuser|all");

                    foreach (string Role in Roles.Split(";,|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries))
                    {
                        if (CurrentUser.IsInRole(Role, SiteContext.CurrentSiteName, true, true))
                        {
                            Authorized = true;
                            break;
                        }
                    }
                }

                // Users
                if (!Authorized && !string.IsNullOrWhiteSpace(Users))
                {
                    OnlyAuthenticatedCheck = false;
                    foreach (string User in Users.Split(";,|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries))
                    {
                        if (User.ToLower().Trim() == CurrentUser.UserName.ToLower().Trim())
                        {
                            Authorized = true;
                            break;
                        }
                    }
                }

                // Explicit Permissions
                if (!Authorized && !string.IsNullOrWhiteSpace(ResourceAndPermissionNames))
                {
                    OnlyAuthenticatedCheck = false;
                    CacheDependencies.Add("cms.role|all");
                    CacheDependencies.Add("cms.userrole|all");
                    CacheDependencies.Add("cms.membershiprole|all");
                    CacheDependencies.Add("cms.membershipuser|all");
                    CacheDependencies.Add("cms.permission|all");
                    CacheDependencies.Add("cms.rolepermission|all");

                    foreach (string ResourcePermissionName in ResourceAndPermissionNames.Split(";,|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries))
                    {
                        string[] StringParts = ResourcePermissionName.Split('.');
                        string PermissionName = StringParts.Last();
                        string ResourceName = string.Join(".", StringParts.Take(StringParts.Length - 1));
                        if (UserSecurityHelper.IsAuthorizedPerResource(ResourceName, PermissionName, SiteContext.CurrentSiteName, CurrentUser))
                        {
                            Authorized = true;
                            break;
                        }
                    }
                }

                // Check page level security
                if (!Authorized && CheckPageACL && FoundPage != null)
                {
                    OnlyAuthenticatedCheck = false;
                    CacheDependencies.Add("cms.role|all");
                    CacheDependencies.Add("cms.userrole|all");
                    CacheDependencies.Add("cms.membershiprole|all");
                    CacheDependencies.Add("cms.membershipuser|all");
                    CacheDependencies.Add("nodeid|" + FoundPage.NodeID);
                    CacheDependencies.Add("documentid|" + FoundPage.DocumentID);
                    CacheDependencies.Add("cms.acl|all");
                    CacheDependencies.Add("cms.aclitem|all");

                    if (TreeSecurityProvider.IsAuthorizedPerNode(FoundPage, NodePermissionToCheck, CurrentUser) != AuthorizationResultEnum.Denied)
                    {
                        Authorized = true;
                    }
                }

                // If there were no other authentication properties, check if this is purely an "just requires authentication" area
                if (OnlyAuthenticatedCheck && (!UserAuthenticationRequired || !CurrentUser.IsPublic()))
                {
                    Authorized = true;
                }

                if (cs.Cached)
                {
                    cs.CacheDependency = CacheHelper.GetCacheDependency(CacheDependencies.Distinct().ToArray());
                }

                return Authorized;
            }, new CacheSettings(CacheAuthenticationResults ? CacheHelper.CacheMinutes(SiteContext.CurrentSiteName) : 0, "AuthorizeCore", CurrentUser.UserID, (FoundPage != null ? FoundPage.DocumentID : -1), SiteContext.CurrentSiteName, Users, Roles, ResourceAndPermissionNames, CheckPageACL, NodePermissionToCheck, CustomUnauthorizedRedirect, UserAuthenticationRequired));
        }

        /// <summary>
        /// Main Authorization method, if not authorized adjusted the filterContext's results to redirect to an Unauthorized result or custom redirect.
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            // If they are authorized, handle accordingly
            if (!AuthorizeCore(filterContext.HttpContext))
            {
                // Custom provided redirect
                if (!string.IsNullOrWhiteSpace(CustomUnauthorizedRedirect))
                {
                    filterContext.Result = new RedirectResult(CustomUnauthorizedRedirect);
                }
                else
                {
                    // Just throw an unauthorzied request
                    filterContext.Result = new HttpUnauthorizedResult();
                }
            }
        }
    }
}