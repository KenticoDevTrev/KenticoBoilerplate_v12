////////////////////////////////////////////////////////////////////////
//////////////////KEEP IN EVERY CUSTOM FORM JAVASCRIPT//////////////////
////////////////////////////////////////////////////////////////////////

$advjQuery = (typeof $cmsj == "undefined" ? jQuery : $cmsj);

// Push this Form's loader function onto stack, last PageLoad loops through it and runs all
if (typeof FormPageLoadFunctions === "undefined") {
    window.FormPageLoadFunctions = new Array();
}
// Add this Form's PageLoad with Key.
window.FormPageLoadFunctions["ACS"] = ACS_PageLoad;

// Use this same pageLoad function for all custom form elements.
function pageLoad() {
    if (typeof window.FormPageLoadFunctions != "undefined") {
        for (var key in window.FormPageLoadFunctions) {
            if (window.FormPageLoadFunctions.hasOwnProperty(key)) {
                window.FormPageLoadFunctions[key]();
            }
        }
    }
}
////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////

function ACS_PageLoad() {
    ACS_SetFormEvents();
}

// Custom ":contains" to make case insensative.
$advjQuery.expr[':'].icontains = function (a, i, m) {
    return $advjQuery(a).text().toLowerCase()
        .indexOf(m[3].toLowerCase()) >= 0;
};

// Sets up the various handlers
function ACS_SetFormEvents() {
    ACS_setEnterClickCategory("categoryFilter", "btnSearch");
    $advjQuery("input[id*='categoryFilter']").keyup(function () {
        ACS_SearchCategories("." + ACS_getContainerClass(this));
    });
    $advjQuery("input[id*='tvwCategoryTree']").unbind("click").click(function () {
        // Set any parent AllChildrenChecked inputs to SomeChildrenChecked
        $advjQuery(this).parents("div[id$='Nodes']").each(function () {
            $advjQuery("input.AllChildrenChecked[id='" + $advjQuery(this).attr("id").replace("Nodes", "CheckBox") + "']").removeClass("AllChildrenChecked").addClass("SomeChildrenChecked");
        });
        if ($advjQuery(this).hasClass("AllChildrenChecked") || $advjQuery(this).hasClass("SomeChildrenChecked")) {
            ACS_handleCheckboxClick(this, $advjQuery(this).hasClass("SomeChildrenChecked"));
        } else {
            ACS_handleCheckboxClick(this, $advjQuery(this).prop("checked"));
        }
        ACS_RegenerateCategoryListValue(this);
    });
}

// Handles logic for checkbox selection, parent-child selecting, etc.
function ACS_handleCheckboxClick(checkboxObj, check) {
    var container = "." + ACS_getContainerClass(checkboxObj);
    var OnlyLeafSelectable = $advjQuery(container + " .tbxOnlyLeafSelectable").val() == "True";
    var ParentSelectsChildren = $advjQuery(container + " .tbxParentSelectsChildren").val() == "True";
    if (OnlyLeafSelectable) {
        if ($advjQuery(container + " #" + $advjQuery(checkboxObj).attr("id").replace("CheckBox", "Nodes")).length > 0) {
            $advjQuery(checkboxObj).prop("checked", check);
            if (check) {
                $advjQuery(checkboxObj).addClass("AllChildrenChecked")
                $advjQuery(checkboxObj).removeClass("SomeChildrenChecked");
            } else {
                $advjQuery(checkboxObj).removeClass("SomeChildrenChecked");
                $advjQuery(checkboxObj).removeClass("AllChildrenChecked");
            }
            var ChildInputs = $advjQuery(container + " #" + $advjQuery(checkboxObj).attr("id").replace("CheckBox", "Nodes") + " > table input");
            for (var i = 0; i < ChildInputs.length; i++) {
                ACS_handleCheckboxClick(ChildInputs[i], check);
            }
        } else {
            $advjQuery(checkboxObj).prop("checked", check);
        }
    }
}


function ACS_setEnterClickCategory(textboxID, buttonID) {
    $advjQuery("*[id*='" + textboxID + "']").keypress(function (e) {
        if (e.which == 13) {
            $advjQuery("*[id*='" + buttonID + "']").trigger("click");
        }
    });
}


function ACS_getContainerClass(obj) {
    return "PageContent";
}

// Searches the text of the form elements in List format.
function ACS_SearchCategories(container) {
    var searchText = $advjQuery(container + " #categoryFilter").val().replace($advjQuery(container + " #categoryFilter").attr("title"), "").toLowerCase();
    if (searchText == "") {
        $advjQuery(container + " div[id*='tvwCategoryTree'] > table").removeClass("Hidden");
    } else {
        $advjQuery(container + " .InputDataPrepend:not(:icontains('" + searchText + "'))").each(function () {
            $advjQuery(this).closest("table").addClass("Hidden");
        });
        $advjQuery(container + " .InputDataPrepend:icontains('" + searchText + "')").each(function () {
            $advjQuery(this).closest("table").removeClass("Hidden");
        });
    }
}

// Generates both the Category IDs and the Display Textbox from the selected items.
function ACS_RegenerateCategoryListValue(obj) {
    var container = "." + ACS_getContainerClass(obj);
    var RegenerateCategoryListValue = "";
    var RegenerateCategoryDisplayListValue = "";
    var SeparatorCharacter = $advjQuery(container + " .tbxSeparatorCharacter").val();
    if (typeof SeparatorCharacter == undefined) {
        SeparatorCharacter = "|";
    }
    $advjQuery(container + " div[id *= 'tvwCategoryTree'] input:checked").each(function () {
        if (!($advjQuery(this).hasClass("AllChildrenChecked") || $advjQuery(this).hasClass("SomeChildrenChecked"))) {
            RegenerateCategoryListValue += SeparatorCharacter + $advjQuery(".InputDataPrepend", $advjQuery(this).parent()).data("value");
            RegenerateCategoryDisplayListValue += SeparatorCharacter + $advjQuery(".InputDataPrepend", $advjQuery(this).parent()).data("text");
        }
    });
    $advjQuery(container + " .tbxCategoryValue").val(RegenerateCategoryListValue.substring(1));
    $advjQuery(container + " .tbxDisplayValueHolder").val(RegenerateCategoryDisplayListValue.substring(1));
}