<%@ Control Language="C#" AutoEventWireup="true" Inherits="Compiled_CMSModules_RelationshipsExtended_Controls_Relateddocs_List" %>
<%@ Register Src="~/CMSModules/RelationshipsExtended/Controls/RelatedDocuments.ascx" TagName="RelatedDocuments" TagPrefix="cms" %>

<asp:Panel ID="pnlContent" runat="server" CssClass="Unsorted">
        <div class="PageContent">
            <cms:MessagesPlaceHolder runat="server" ID="plcMessages" />
            <cms:RelatedDocuments ID="relatedDocuments" runat="server" ShowAddRelation="false" IsLiveSite="false" PageSize="10,25,50,100,##ALL##" DefaultPageSize="25" />
        </div>
    </asp:Panel>
