<%@ Control Language="C#" AutoEventWireup="true" Inherits="Compiled_CMSModules_RelationshipsExtended_FormControls_Relationships_RelatedDocuments_RE" %>
<%@ Register Src="~/CMSModules/RelationshipsExtended/UI/UniGrid/UniGrid.ascx" TagName="UniGrid" TagPrefix="cms" %>
<!-- This is a special control for page types -->
<div>
    <asp:Panel ID="pnlNewLink" runat="server" Style="margin-bottom: 8px;">
        <cms:LocalizedButton runat="server" ID="btnNewRelationship" ButtonStyle="Default" ResourceString="relationship.addrelateddocs" EnableViewState="false" CssClass="btn-group" />
    </asp:Panel>
    <div>
        <cms:CMSUpdatePanel ID="pnlUpdate" runat="server" class="">
            <ContentTemplate>
                <cms:MessagesPlaceHolder ID="plcMess" runat="server" />
                <cms:UniGrid ID="UniGridRelationship" runat="server" GridName="~/CMSModules/RelationshipsExtended/FormControls/Relationships/RelatedDocuments_RE_List.xml"
                    OrderBy="RelationshipNameID" IsLiveSite="false" ShowObjectMenu="false" />
            </ContentTemplate>
        </cms:CMSUpdatePanel>
    </div>
    <asp:HiddenField ID="hdnSelectedNodeId" runat="server" Value="" />
</div>
