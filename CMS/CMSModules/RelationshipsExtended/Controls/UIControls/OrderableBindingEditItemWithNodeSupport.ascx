<%@ Control Language="C#" AutoEventWireup="true" Inherits="Compiled_CMSModules_RelationshipsExtended_Controls_UIControls_NodeBindingEditItem" %>
<%@ Register Src="~/CMSModules/RelationshipsExtended/UI/OrderableUniSelector/UniSelector.ascx" TagPrefix="RelationshipsExtended" TagName="OrderableUniSelector" %>
<RelationshipsExtended:OrderableUniSelector runat="server" ID="editElem" IsLiveSite="false" SelectionMode="Multiple"  />
<style>
.cms-bootstrap .btn.icon-only.js-custom_move {
    cursor: move;
}
</style>