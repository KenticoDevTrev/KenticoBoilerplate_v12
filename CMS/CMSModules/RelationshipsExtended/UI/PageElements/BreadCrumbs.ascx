<%@ Control Language="C#" AutoEventWireup="false" Inherits="Compiled_CMSModules_RelationshipsExtended_UI_PageElements_BreadCrumbs" %>
<%@ Register Src="~/CMSModules/RelationshipsExtended/UI/PageElements/Help.ascx" TagName="Help" TagPrefix="cms" %>
<asp:Panel runat="server" CssClass="breadcrumbs-dialog" ID="pnlBreadCrumbs">
    <ul class="breadcrumb">
        <asp:PlaceHolder ID="plcBreadcrumbs" runat="server" />
    </ul>
    <div class="breadcrumb-help">
        <cms:Help ID="helpBreadcrumbs" runat="server" Visible="false" />
    </div>
</asp:Panel>