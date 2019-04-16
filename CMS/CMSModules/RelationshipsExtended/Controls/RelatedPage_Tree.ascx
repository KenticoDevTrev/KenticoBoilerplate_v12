<%@ Control Language="C#" AutoEventWireup="true" Inherits="Compiled_CMSModules_RelationshipsExtended_Controls_RelatedPage_Tree" %>
<asp:Button runat="server" ID="btnAdd" CssClass="btn btn-primary AddButton" Text="Add Selected Pages" OnClick="btnAdd_Click" />
<asp:DropDownList runat="server" ID="ddlCurrentNodeDirection" CssClass="DropDownField form-control DirectionSelector" AutoPostBack="true" OnSelectedIndexChanged="ddlCurrentNodeDirection_SelectedIndexChanged">
    <asp:ListItem Value="LeftNode">Add as Right-side Page</asp:ListItem>
    <asp:ListItem Value="RightNode">Add as Left-side Page</asp:ListItem>
</asp:DropDownList>
<div class="col-md-12 ContentTree" style="text-align: left">
    <asp:TreeView runat="server" ID="pageTree" />
</div>
<style>
    .Selectable {
      float: left;
      display: inline-block;
      padding-right: 5px;
      font-weight: bold;
      cursor: pointer;
    }
    .AlreadySelected {
        color: grey;
    }
    input[type='checkbox'] {
        cursor: pointer;
    }
    .cms-bootstrap .DirectionSelector {
      display: inline-block !important;
      width: auto;
      position: relative;
      top: 3px;
    }
    
    /* Special Display classes for settings groups */
    .ContentTree td:not(.InputNode) {
        vertical-align: top;
    }
    .ContentTree td.InputNode {
        vertical-align: middle;
    }
    .ms-choice, .ms-parent {
        height: 25px;
        margin: 5px 0;
        width: 100% !important;
    }
    .Selectable a {
      text-decoration: underline !important;
      color: blue !important;
    }
</style>
<script type="text/javascript">
    (function ($) {
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(SetClickableCheckboxes);
        
        function SetClickableCheckboxes() {
            if ($(".InputNode input[type='checkbox']").length > 0) {
                $(".InputNode .Selectable").click(function () {
                    var Checkbox = $("input[type='checkbox']", $(this).closest(".InputNode"));
                    Checkbox.prop("checked", !Checkbox.is(":checked"));
                });
            }
        }

        $(document).ready(function () {
            SetClickableCheckboxes();
        });
    })((typeof $cmsj !== "undefined" ? $cmsj : (typeof $ !== "undefined" ? $ : jQuery)));

</script>
