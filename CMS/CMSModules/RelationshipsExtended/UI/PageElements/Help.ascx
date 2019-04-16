<%@ Control Language="C#" AutoEventWireup="true" Inherits="Compiled_CMSModules_RelationshipsExtended_UI_PageElements_Help" EnableViewState="false" %>

<asp:HyperLink runat="server" ID="lnkHelp" CssClass="hidden" Target="_blank">
    <asp:PlaceHolder runat="server" ID="plcIcon">
        <span class="sr-only"><%= GetString("help.tooltip") %></span>
        <i id="iconHelp" runat="server" aria-hidden="true"></i>
    </asp:PlaceHolder>
</asp:HyperLink>