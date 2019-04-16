<%@ Control Language="C#" AutoEventWireup="true" Inherits="Compiled_CMSModules_RelationshipsExtended_Controls_RelatedCategories" %>
<style>
.ContentTree {
  padding-top: 20px;
  padding-bottom: 20px;
}
.icon-modal-close {
        background-color: transparent;
        border: medium none;
}

input.AllChildrenChecked {
    opacity: 1;
}

input.SomeChildrenChecked {
    opacity: .5;
}


.ms-choice, .ms-parent {
    height: 25px;
    margin: 5px 0;
    width: 100% !important;
}

.textFilterAndButton input{
    margin: 10px 0;
}

/* Special Display classes for settings groups */
.cms-bootstrap .ContentTree td:not(.InputNode) {
  vertical-align: top;
}
.cms-bootstrap .ContentTree td.InputNode {
  vertical-align: middle;
}

div.InputDataPrepend {
    display: inline-block !important;
}
.cms-bootstrap .btn.full-width {
    width: 100%;
}
</style>
<!-- Actual Tool -->
<asp:UpdatePanel ID="pnlCategoryExpandContract" runat="server">
    <ContentTemplate>
        <div class="amFormTool">
            <div class="row amHeader">
                <div class="col-md-12">

                    <asp:Panel ID="pnlSearchFilter" runat="server">
                        <div class="row textFilterAndButton">
                            <div class="col-md-12">
                                <input id="categoryFilter" placeholder="Filter by Title" title="Filter by Title" type="text" style="width: 100%;" />
                            </div>
                            <div class="hidden Hidden">
                                <asp:Button ID="btnSearch" CausesValidation="false" runat="server" Text="Search" />
                            </div>
                        </div>
                    </asp:Panel>
                    <asp:Panel ID="pnlTreeButtons" runat="server">
                        <div class="row textFilterAndButton">
                            <div class="col-sm-4">
                                <asp:Button ID="btnExpandChecked" CssClass="btn btn-default full-width" CausesValidation="false" OnClick="btnExpandAll_Click" runat="server" Text="Expand All" />
                            </div>
                            <div class="col-sm-4">
                                <asp:Button ID="btnExpandAll" CssClass="btn btn-default full-width" CausesValidation="false" OnClick="btnExpandChecked_Click" runat="server" Text="Show Selected" />
                            </div>
                            <div class="col-sm-4">
                                <asp:Button ID="btnCollapseAll" CssClass="btn btn-default full-width" CausesValidation="false" OnClick="btnCollapseAll_Click" runat="server" Text="Collapse All" />
                            </div>
                        </div>
                    </asp:Panel>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12 ContentTree" style="text-align: left">
                    <!-- Tree list here -->
                    <asp:TreeView ID="tvwCategoryTree" runat="server" OnTreeNodeDataBound="tvwCategoryTree_TreeNodeDataBound" />
                </div>
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
<asp:Button runat="server" ID="btnAddCategories" Text="Set Categories" CssClass="btn btn-primary" OnClick="btnAddCategories_Click" />
<div style="display: none;">
    <!-- placed here so javascript can access the configuration values -->
    <asp:TextBox ID="tbxOnlyLeafSelectable" runat="server" Text="" CssClass="tbxOnlyLeafSelectable" />
    <asp:TextBox ID="tbxParentSelectsChildren" runat="server" Text="" CssClass="tbxParentSelectsChildren" />
</div>
