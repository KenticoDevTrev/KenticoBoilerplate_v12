<%@ Control Language="C#" AutoEventWireup="true" Inherits="Compiled_CMSModules_RelationshipsExtended_FormControls_AdvancedCategorySelector_AdvancedCategorySelector" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<cms:CMSUpdatePanel ID="pnlUpdate" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div class="control-group-inline AdvancedCategorySelector">
            <div class="form-group">
                <asp:TextBox ID="tbxDisplayValue" runat="server" CssClass="form-control tbxDisplayValue" />
                <cms:CMSTextBox ID="txtValue" runat="server" OnTextChanged="txtValue_TextChanged" CssClass="form-control" />
                <asp:Button ID="btnShow" runat="server" Text="Select Categories" CssClass="btn btn-default" />
            </div>
            <!-- ModalPopupExtender -->
            <cc1:ModalPopupExtender ID="mp1" runat="server" PopupControlID="Panel1" TargetControlID="btnShow" CancelControlID="btnClose" BackgroundCssClass="modalBackground"></cc1:ModalPopupExtender>
            <asp:Panel ID="Panel1" runat="server" CssClass="modalPopup" align="center" Style="display: none">
                <asp:Panel ID="ModalContainerForClass" runat="server">
                    <div class="CustomModal">
                        <div id="m_pnlBody" class="TabsPageHeader">
                            <div id="m_pnlTitle" class="DialogsPageHeader">
                                <div id="m_pt_pnlBody">
                                    <div id="m_pt_pnlTitle" class="dialog-header non-selectable" style="cursor: move;">
                                        <h2 id="m_pt_headTitle" class="dialog-header-title" style="white-space: nowrap;">Select Categories
                                        </h2>
                                    </div>
                                </div>
                            </div>
                            <div id="m_pnlSeparator" class="HeaderSeparator">&nbsp;</div>
                            <div class="DialogMainBlock" style="height: 581px;">
                                <div class="DialogContent">

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
                                                                    <asp:Button ID="btnExpandChecked" CausesValidation="false" OnClick="btnExpandAll_Click" runat="server" Text="Expand All" />
                                                                </div>
                                                                <div class="col-sm-4">
                                                                    <asp:Button ID="btnExpandAll" CausesValidation="false" OnClick="btnExpandChecked_Click" runat="server" Text="Show Selected" />
                                                                </div>
                                                                <div class="col-sm-4">
                                                                    <asp:Button ID="btnCollapseAll" CausesValidation="false" OnClick="btnCollapseAll_Click" runat="server" Text="Collapse All" />
                                                                </div>
                                                            </div>
                                                        </asp:Panel>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-12" style="text-align: left">
                                                        <!-- Tree list here -->
                                                        <asp:TreeView ID="tvwCategoryTree" runat="server" OnTreeNodeDataBound="tvwCategoryTree_TreeNodeDataBound" />
                                                    </div>
                                                </div>
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                    <div style="display: none;">
                                        <!-- placed here so javascript can access the configuration values -->
                                        <asp:TextBox ID="tbxSaveMode" runat="server" Text="" CssClass="tbxSaveMode" />
                                        <asp:TextBox ID="tbxDisplayValueHolder" runat="server" Text="" CssClass="tbxDisplayValueHolder" />
                                        <asp:TextBox ID="tbxOnlyLeafSelectable" runat="server" Text="" CssClass="tbxOnlyLeafSelectable" />
                                        <asp:TextBox ID="tbxParentSelectsChildren" runat="server" Text="" CssClass="tbxParentSelectsChildren" />

                                        <!-- Used to store values for the javascript -->
                                        <asp:TextBox ID="tbxSeparatorCharacter" runat="server" CssClass="tbxSeparatorCharacter" />
                                        <asp:TextBox ID="tbxCategoryValue" runat="server" Text="" CssClass="tbxCategoryValue" />
                                        <asp:Literal ID="ltrOriginalValues" runat="server" Text="" />
                                        
                                    </div>
                                </div>
                            </div>
                            <div class="PageFooterLine">
                                <div class="FloatRight">
                                    <asp:Button ID="btnSelect" runat="server" CausesValidation="false" Text="Select" CssClass="btn btn-primary" />
                                    <asp:Button ID="btnClose" runat="server" CausesValidation="false" Text="Cancel" CssClass="btn btn-primary" />
                                </div>
                            </div>
                        </div>

                    </div>

                </asp:Panel>
            </asp:Panel>
        </div>
    </ContentTemplate>
</cms:CMSUpdatePanel>
