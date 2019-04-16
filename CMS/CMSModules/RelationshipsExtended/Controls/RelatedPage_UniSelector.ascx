<%@ Control Language="C#" AutoEventWireup="true" Inherits="Compiled_CMSModules_RelationshipsExtended_Controls_RelatedPage_UniSelector" %>
<%@ Register Src="~/CMSModules/RelationshipsExtended/UI/UniSelector/UniSelector.ascx" TagName="UniSelector" TagPrefix="RelExt" %>

<asp:DropDownList runat="server" ID="ddlCurrentNodeDirection" CssClass="DropDownField form-control DirectionSelector" AutoPostBack="true" OnSelectedIndexChanged="ddlCurrentNodeDirection_SelectedIndexChanged">
    <asp:ListItem Value="LeftNode">Add as Right-side Page</asp:ListItem>
    <asp:ListItem Value="RightNode">Add as Left-side Page</asp:ListItem>
</asp:DropDownList>
<RelExt:UniSelector runat="server" CssClass="AddButton" ID="CustomUniSelector" ObjectType="CMS.Node" ReturnColumnName="NodeID" OnOnSelectionChanged="CustomUniSelector_OnSelectionChanged" />
<style>
    .cms-bootstrap .DirectionSelector {
      display: inline-block !important;
      width: auto;
      position: relative;
      top: 3px;
    }
    .cms-bootstrap .AddButton {
        display: inline-block;
    }
</style>