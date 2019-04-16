<%@ Control Language="C#" AutoEventWireup="true" Inherits="Compiled_CMSModules_RelationshipsExtended_Controls_RelatedDocuments" %>
<%@ Register Src="~/CMSModules/RelationshipsExtended/UI/UniGrid/UniGrid.ascx" TagName="UniGrid" TagPrefix="cms" %>
<div>
    <cms:CMSUpdatePanel ID="pnlUpdate" runat="server" class="">
        <ContentTemplate>
            <cms:MessagesPlaceHolder ID="plcMess" runat="server" />
            <cms:UniGrid ID="UniGridRelationship" runat="server" GridName="~/CMSModules/RelationshipsExtended/Grids/RelatedDocuments_List.xml" OrderBy="RelationshipNameID" IsLiveSite="false" />
        </ContentTemplate>
    </cms:CMSUpdatePanel>
    <asp:HiddenField ID="hdnSelectedNodeId" runat="server" Value="" />
</div>
<style>
    .CustomUniGridEntry {
      cursor: default;
    }
</style>