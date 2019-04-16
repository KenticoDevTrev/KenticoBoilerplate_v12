<%@ Control Language="C#" AutoEventWireup="true" Inherits="Compiled_CMSModules_RelationshipsExtended_Controls_Basic_OrderByControl" %>
<cms:CMSUpdatePanel ID="pnlUpdate" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:HiddenField ID="hdnIndices" runat="server" />
        <asp:PlaceHolder runat="server" ID="plcOrderBy" />
    </ContentTemplate>
</cms:CMSUpdatePanel>
