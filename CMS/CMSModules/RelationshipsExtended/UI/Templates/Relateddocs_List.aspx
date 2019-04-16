<%@ Page Language="C#" AutoEventWireup="true" Inherits="Compiled_CMSModules_RelationshipsExtended_UI_Templates_Relateddocs_List" Theme="Default" MaintainScrollPositionOnPostback="true" MasterPageFile="~/CMSMasterPages/UI/SimplePage.master" %>
<%@ Register Src="~/CMSModules/RelationshipsExtended/FormControls/Relationships/RelatedDocuments.ascx" TagName="RelatedDocuments" TagPrefix="cms" %>

<asp:Content ID="cntBody" runat="server" ContentPlaceHolderID="plcContent">
    <asp:Panel ID="pnlContent" runat="server" CssClass="Unsorted">
        <div class="PageContent">
            <cms:MessagesPlaceHolder runat="server" ID="plcMessages" />
            <cms:RelatedDocuments ID="relatedDocuments" runat="server" ShowAddRelation="false"
                IsLiveSite="false" PageSize="10,25,50,100,##ALL##" DefaultPageSize="25" />
        </div>
    </asp:Panel>
</asp:Content>
