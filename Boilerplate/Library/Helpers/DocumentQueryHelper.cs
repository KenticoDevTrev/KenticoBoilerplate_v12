using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.Helpers;
using CMS.SiteProvider;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace KMVCHelper
{
    public class DocumentQueryHelper
    {

        /// <summary>
        /// Gets the TreeNode for the corresponding path, can be either the NodeAliasPath or a URL Alias
        /// </summary>
        /// <param name="Path"></param>
        /// <returns></returns>
        public static TreeNode GetNodeByAliasPath(string Path, string ClassName = null, string CultureCode = null)
        {
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
                        CacheDependencies.Add(string.Format("node|{0}|{1}", EnvironmentHelper.CurrentSiteName, Path));
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
            }, new CacheSettings(CacheHelper.CacheMinutes(EnvironmentHelper.CurrentSiteName), Path, ClassName, CultureCode));
        }

        /// <summary>
        ///  DocumentQuery that mimics the Hierarchy Viewer webpart, with caching included.
        /// </summary>
        /// <param name="Path">The Path for the documents to select</param>
        /// <param name="ClassNames">Class Names to include, semicolon seperated.</param>
        /// <param name="CombineWithDefaultCulture"></param>
        /// <param name="CultureCode"></param>
        /// <param name="MaxRelativeLevel"></param>
        /// <param name="UseHierarchicalOrder">Ensures the NodeLevel, NodeOrder is set first as the order.</param>
        /// <param name="AdditionalOrderBy"></param>
        /// <param name="SelectOnlyPublished"></param>
        /// <param name="SiteName"></param>
        /// <param name="WhereCondition"></param>
        /// <param name="Columns"></param>
        /// <param name="FilterOutDuplicates"></param>
        /// <param name="RelationshipWithNodeGuid">The Relationship to use, defaults to the current page.</param>
        /// <param name="RelatedNodeIsOnTheLeftSide"></param>
        /// <param name="RelationshipName"></param>
        /// <param name="CheckPermission"></param>
        /// <param name="LoadPagesIndividually"></param>
        /// <param name="CacheItemName">The Cache Item Name, required if you wish to cache.</param>
        /// <param name="CacheMinutes">The Cache minutes, if not provided uses the site default.</param>
        /// <param name="CacheDependencies">Any cache dependencies</param>
        /// <returns></returns>
        public static CacheableDocumentQuery HierarchyViewerQuery(
            string Path = "/%",
            string ClassNames = null,
            bool CombineWithDefaultCulture = false,
            string CultureCode = null,
            int MaxRelativeLevel = -1,
            bool UseHierarchicalOrder = true,
            string AdditionalOrderBy = null,
            bool SelectOnlyPublished = true,
            string SiteName = null,
            string WhereCondition = null,
            string Columns = null,
            bool FilterOutDuplicates = false,
            Guid RelationshipWithNodeGuid = new Guid(),
            bool RelatedNodeIsOnTheLeftSide = true,
            string RelationshipName = null,
            bool CheckPermission = false,
            bool LoadPagesIndividually = false,
            string CacheItemName = null,
            int CacheMinutes = -1,
            string[] CacheDependencies = null
            )
        {
            AdditionalOrderBy = !string.IsNullOrWhiteSpace(AdditionalOrderBy) ? AdditionalOrderBy : "1";
            if (UseHierarchicalOrder)
            {
                AdditionalOrderBy = SqlHelper.AddOrderBy(AdditionalOrderBy, "NodeLevel, NodeOrder");
            }
            return RepeaterQuery(
                Path: Path,
                ClassNames: ClassNames,
                CombineWithDefaultCulture: CombineWithDefaultCulture,
                CultureCode: CultureCode,
                MaxRelativeLevel: MaxRelativeLevel,
                OrderBy: AdditionalOrderBy,
                SelectOnlyPublished: SelectOnlyPublished,
                SelectTopN: -1,
                SiteName: SiteName,
                WhereCondition: WhereCondition,
                Columns: Columns,
                FilterOutDuplicates: FilterOutDuplicates,
                RelationshipWithNodeGuid: RelationshipWithNodeGuid,
                RelatedNodeIsOnTheLeftSide: RelatedNodeIsOnTheLeftSide,
                RelationshipName: RelationshipName,
                CheckPermission: CheckPermission,
                LoadPagesIndividually: LoadPagesIndividually,
                CacheItemName: CacheItemName,
                CacheMinutes: CacheMinutes,
                CacheDependencies: CacheDependencies);
        }

        /// <summary>
        /// DocumentQuery that mimics the Repeater Webpart, with caching included.
        /// </summary>
        /// <param name="Path"></param>
        /// <param name="NodeID">The NodeID</param>
        /// <param name="DocumentID">The DocumentID</param>
        /// <param name="ClassNames"></param>
        /// <param name="CombineWithDefaultCulture"></param>
        /// <param name="CultureCode"></param>
        /// <param name="MaxRelativeLevel"></param>
        /// <param name="OrderBy"></param>
        /// <param name="SelectOnlyPublished"></param>
        /// <param name="SelectTopN"></param>
        /// <param name="SiteName"></param>
        /// <param name="WhereCondition"></param>
        /// <param name="Columns"></param>
        /// <param name="FilterOutDuplicates"></param>
        /// <param name="RelationshipWithNodeGuid"></param>
        /// <param name="RelatedNodeIsOnTheLeftSide"></param>
        /// <param name="RelationshipName"></param>
        /// <param name="CheckPermission"></param>
        /// <param name="LoadPagesIndividually"></param>
        /// <param name="CacheItemName">The Cache Item Name, required if you wish to cache.</param>
        /// <param name="CacheMinutes">The Cache minutes, if not provided uses the site default.</param>
        /// <param name="CacheDependencies">Any cache dependencies</param>
        /// <returns></returns>
        public static CacheableDocumentQuery RepeaterQuery(
            string Path = "/%",
            int NodeID = -1,
            int DocumentID = -1,
            string ClassNames = null,
            bool CombineWithDefaultCulture = false,
            string CultureCode = null,
            int MaxRelativeLevel = -1,
            string OrderBy = null,
            bool SelectOnlyPublished = true,
            int SelectTopN = -1,
            string SiteName = null,
            string WhereCondition = null,
            string Columns = null,
            bool FilterOutDuplicates = false,
            Guid RelationshipWithNodeGuid = new Guid(),
            bool RelatedNodeIsOnTheLeftSide = true,
            string RelationshipName = null,
            bool CheckPermission = false,
            bool LoadPagesIndividually = false,
            string CacheItemName = null,
            int CacheMinutes = -1,
            string[] CacheDependencies = null)
        {
            CacheableDocumentQuery RepeaterQuery = (!string.IsNullOrWhiteSpace(ClassNames) ? new CacheableDocumentQuery(ClassNames) : new CacheableDocumentQuery());
            if (string.IsNullOrWhiteSpace(Path))
            {
                Path = "/%";
            }
            RepeaterQuery.Path(Path);
            RepeaterQuery.CacheItemNameParts.Add(Path);

            if (NodeID > 0)
            {
                RepeaterQuery.WhereEquals("NodeID", NodeID);
                RepeaterQuery.CacheItemNameParts.Add(NodeID);
                RepeaterQuery.CacheDependencyParts.Add("nodeid|" + NodeID);
            }
            if (DocumentID > 0)
            {
                RepeaterQuery.WhereEquals("DocumentID", DocumentID);
                RepeaterQuery.CacheItemNameParts.Add(DocumentID);
                RepeaterQuery.CacheDependencyParts.Add("documentid|" + DocumentID);
            }

            RepeaterQuery.CombineWithDefaultCulture(CombineWithDefaultCulture);
            if (string.IsNullOrWhiteSpace(CultureCode))
            {
                CultureCode = EnvironmentHelper.CurrentCulture;
            }
            if (!string.IsNullOrWhiteSpace(CultureCode))
            {
                RepeaterQuery.Culture(CultureCode);
                RepeaterQuery.CacheItemNameParts.Add(CultureCode);
            }
            if (EnvironmentHelper.PreviewEnabled)
            {
                RepeaterQuery.LatestVersion(true);
                RepeaterQuery.Published(false);
            }
            else
            {
                RepeaterQuery.PublishedVersion(true);
            }
            if (MaxRelativeLevel > -1)
            {
                RepeaterQuery.NestingLevel(MaxRelativeLevel);
                RepeaterQuery.CacheItemNameParts.Add(MaxRelativeLevel);
            }
            if (!string.IsNullOrWhiteSpace(OrderBy))
            {
                RepeaterQuery.OrderBy(OrderBy);
                RepeaterQuery.CacheItemNameParts.Add(OrderBy);
            }
            RepeaterQuery.Published(SelectOnlyPublished);
            RepeaterQuery.CacheItemNameParts.Add(SelectOnlyPublished);

            if (SelectTopN > -1)
            {
                RepeaterQuery.TopN(SelectTopN);
                RepeaterQuery.CacheItemNameParts.Add(SelectTopN);
            }
            if (string.IsNullOrWhiteSpace(SiteName))
            {
                SiteName = EnvironmentHelper.CurrentSiteName;
            }
            if (!string.IsNullOrWhiteSpace(SiteName))
            {
                RepeaterQuery.OnSite(new SiteInfoIdentifier(SiteName));
                RepeaterQuery.CacheItemNameParts.Add(SiteName);
            }
            if (!string.IsNullOrWhiteSpace(WhereCondition))
            {
                RepeaterQuery.Where(WhereCondition);
                RepeaterQuery.CacheItemNameParts.Add(WhereCondition);
            }
            if (!string.IsNullOrWhiteSpace(Columns))
            {
                RepeaterQuery.Columns(Columns);
                RepeaterQuery.CacheItemNameParts.Add(Columns);
            }
            RepeaterQuery.FilterDuplicates(FilterOutDuplicates);
            if (!string.IsNullOrWhiteSpace(RelationshipName))
            {
                RepeaterQuery.InRelationWith(RelationshipWithNodeGuid, RelationshipName, (RelatedNodeIsOnTheLeftSide ? RelationshipSideEnum.Left : RelationshipSideEnum.Right));
                RepeaterQuery.CacheItemNameParts.Add(RelationshipName);
                RepeaterQuery.CacheItemNameParts.Add(RelationshipWithNodeGuid);
                RepeaterQuery.CacheItemNameParts.Add(RelatedNodeIsOnTheLeftSide);
            }

            // Handle Caching params
            RepeaterQuery.CacheMinutes = CacheMinutes;
            if (!string.IsNullOrWhiteSpace(CacheItemName))
            {
                RepeaterQuery.CacheItemNameParts.Add(CacheItemName);
            }
            return RepeaterQuery;
        }
    }

    /// <summary>
    /// DocumentQuery class with Caching taken into affect
    /// </summary>
    public class CacheableDocumentQuery : DocumentQuery
    {
        public string CacheItemName;
        public int CacheMinutes;
        public string[] CacheDependencies;
        /// <summary>
        /// Items that are used as part of the CacheItemName, and create a unique cache name.
        /// </summary>
        public List<object> CacheItemNameParts;
        /// <summary>
        /// Any additional Cache Dependencies, these are often added automatically for things like NodeID or DocumentID
        /// </summary>
        public List<string> CacheDependencyParts;
        public CacheableDocumentQuery() : base()
        {
            CacheItemNameParts = new List<object>();
            CacheDependencyParts = new List<string>();
        }
        public CacheableDocumentQuery(string className) : base(className)
        {
            CacheItemNameParts = new List<object>();
            CacheDependencyParts = new List<string>();
        }

        /// <summary>
        /// Gets the Results (DataSet) of the results, caching if applicable
        /// </summary>
        /// <returns>The DataSet of the results</returns>
        public DataSet GetCachedResult()
        {
            if (CacheMinutes == -1)
            {
                // Check cache settings
                CacheMinutes = CacheHelper.CacheMinutes(EnvironmentHelper.CurrentSiteName);
            }
            if (string.IsNullOrWhiteSpace(CacheItemName))
            {
                CacheItemName = string.Join("|", CacheItemNameParts);
            }
            if (CacheMinutes > 0 && !string.IsNullOrWhiteSpace(CacheItemName))
            {
                return CacheHelper.Cache<DataSet>(cs =>
                {
                    if (cs.Cached)
                    {
                        if (CacheDependencies != null)
                        {
                            CacheDependencyParts.AddRange(CacheDependencies);
                        }
                        cs.CacheDependency = CacheHelper.GetCacheDependency(CacheDependencyParts.Distinct().ToArray());
                    }
                    return Result;
                }, new CacheSettings(CacheMinutes, CacheItemName, "TreeNode_Result", CacheItemNameParts));
            }
            else
            {
                return Result;
            }
        }

        /// <summary>
        /// Gets the Typed Results of the query with caching
        /// </summary>
        /// <returns>The Typed Results.</returns>
        public InfoDataSet<TreeNode> GetTypedResult()
        {
            if (CacheMinutes == -1)
            {
                // Check cache settings
                CacheMinutes = CacheHelper.CacheMinutes(EnvironmentHelper.CurrentSiteName);
            }
            if (string.IsNullOrWhiteSpace(CacheItemName))
            {
                CacheItemName = string.Join("|", CacheItemNameParts);
            }
            if (CacheMinutes > 0 && !string.IsNullOrWhiteSpace(CacheItemName))
            {
                return CacheHelper.Cache<InfoDataSet<TreeNode>>(cs =>
                {
                    if (cs.Cached)
                    {
                        if (CacheDependencies != null)
                        {
                            CacheDependencyParts.AddRange(CacheDependencies);
                        }
                        cs.CacheDependency = CacheHelper.GetCacheDependency(CacheDependencyParts.Distinct().ToArray());
                    }
                    return TypedResult;
                }, new CacheSettings(CacheMinutes, CacheItemName, "TreeNode_TypedResult", CacheItemNameParts));
            }
            else
            {
                return TypedResult;
            }
        }

        /// <summary>
        /// Gets the Typed Results, populating the "Children" of the Nodes with their children.
        /// </summary>
        /// <returns>The TreeNodes (top level) with Children populated with their child elements.</returns>
        public InfoDataSet<TreeNode> GetHierarchicalTypedResult()
        {
            if (CacheMinutes == -1)
            {
                // Check cache settings
                CacheMinutes = CacheHelper.CacheMinutes(EnvironmentHelper.CurrentSiteName);
            }
            if (string.IsNullOrWhiteSpace(CacheItemName))
            {
                CacheItemName = string.Join("|", CacheItemNameParts);
            }
            if (CacheMinutes > 0 && !string.IsNullOrWhiteSpace(CacheItemName))
            {
                return CacheHelper.Cache<InfoDataSet<TreeNode>>(cs =>
                {
                    Dictionary<int, TreeNode> ParentNodeIDToTreeNode = new Dictionary<int, TreeNode>();
                    List<TreeNode> CompiledNodes = new List<TreeNode>();
                    if (cs.Cached)
                    {
                        if (CacheDependencies != null)
                        {
                            CacheDependencyParts.AddRange(CacheDependencies);
                        }
                        cs.CacheDependency = CacheHelper.GetCacheDependency(CacheDependencyParts.Distinct().ToArray());
                    }
                    // Populate the Children of the TypedResults
                    foreach (TreeNode Node in TypedResult)
                    {
                        // Make sure Children only contain the children found in the typed results.
                        Node.Children.MakeEmpty();
                        ParentNodeIDToTreeNode.Add(Node.NodeID, Node);
                        // If no parent exists, add to top level
                        if (!ParentNodeIDToTreeNode.ContainsKey(Node.NodeParentID))
                        {
                            CompiledNodes.Add(Node);
                        }
                        else
                        {
                            // Otherwise, add to the parent element.
                            ParentNodeIDToTreeNode[Node.NodeParentID].Children.Add(Node);
                        }
                    }
                    return new InfoDataSet<TreeNode>(CompiledNodes.ToArray());
                }, new CacheSettings(CacheMinutes, CacheItemName, "TreeNode_TypedResult", CacheItemNameParts));
            }
            else
            {
                return TypedResult;
            }
        }
    }
}
