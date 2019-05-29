
function InitalizeTinyMCEInputComponent(TextAreaName, HiddenElemName) {
    var editor = document.querySelector("[data-name='" + TextAreaName+"'");
    var htmlContentInput = document.querySelector("[data-name='" + HiddenElemName + "'");
    if (document.getElementById("TinyMCEEditorContainer") === null) {
        var div = document.createElement("div");
        div.id = "TinyMCEEditorContainer";
        div.style = "position: fixed; top:0; left: 0; right: 0; z-index: 20000;";
        document.getElementsByTagName("body")[0].prepend(div);
    }
    
    var config = {
        target: editor,
        inline: true,
        plugins: [editor.dataset.plugins],
        toolbar: editor.dataset.tools,
        skin_url: '/Content/CSS/skins/ui/oxide',
        content_css: '/Content/CSS/skins/content/default/content.min.css',
        fixed_toolbar_container: '#TinyMCEEditorContainer'
    };
    
    config.init_instance_callback = function (tinyMceEditor) {
        tinyMceEditor.on('Change', function (e) {
            var HtmlContent = e.target.getContent();
            htmlContentInput.value = HtmlContent;
        });
       
    };

    config.setup = function (cfgEditor) {

        //Attribution logo and button added because inline editor still needs to display link to tinymce site if we want to use it
        cfgEditor.ui.registry.addIcon('tinymce-toolbar-attribution',
            '<svg xmlns="http://www.w3.org/2000/svg" xmlns: xlink="http://www.w3.org/1999/xlink" width="33px" height="34px" viewBox="0 0 410 447" version="1.1"><defs /><g id="Page-1" stroke="none" stroke-width="1" fill="none" fill-rule="evenodd"><g id="Group" fill="#1975D1"><path d="M180.94,72.67 L233.13,72.67 L233.13,84.3 L180.94,84.3 L180.94,72.67 Z M180.94,142.44 L233.13,142.44 L233.13,154.07 L180.94,154.07 L180.94,142.44 Z M163.55,119.18 L250.55,119.18 L250.55,130.81 L163.55,130.81 L163.55,119.18 Z M163.55,95.93 L250.55,95.93 L250.55,107.56 L163.55,107.56 L163.55,95.93 Z M111.36,110.47 L207.07,203.47 L302.7,110.47 L207.08,17.47 L111.36,110.47 Z M206.65,0 L311.4,101 L326.58,86.82 L337.49,96.71 L207,223.83 L100.89,120.66 L86.83,134.23 L76.57,124.28 L206.65,0 Z" id="Shape" /><path d="M335.25,395.79 L281.25,268.05 L300,268.05 L344.08,373.33 L391.21,268.05 L410,268.05 L329.68,446.5 L311.18,446.5 L335.25,395.79 Z M150,268.05 L167.74,268.05 L167.74,285 L168.25,285 C169.931424,282.682867 171.793927,280.502701 173.82,278.48 C176.456346,275.868252 179.441302,273.633741 182.69,271.84 C186.580131,269.687295 190.703555,267.986256 194.98,266.77 C200.131708,265.334286 205.462516,264.643942 210.81,264.72 C218.802703,264.627262 226.759125,265.808742 234.38,268.22 C241.997925,270.881736 248.923772,275.214234 254.65,280.9 C260.155511,286.138214 264.437308,292.526765 267.19,299.61 C270.31,307.256667 271.873333,316.553333 271.88,327.5 L271.88,400.19 L254.09,400.19 L254.09,327.45 C254.09,318.783333 252.866667,311.45 250.42,305.45 C248.348283,300.054783 245.103386,295.187437 240.92,291.2 C236.454646,286.939924 231.017474,283.833459 225.08,282.15 C220.424954,280.772834 215.612174,279.999831 210.76,279.85 C205.911186,280.000685 201.101821,280.773679 196.45,282.15 C190.512526,283.833459 185.075354,286.939924 180.61,291.2 C176.546971,295.09698 173.389273,299.838647 171.36,305.09 C168.906667,310.963333 167.683333,318.006667 167.69,326.22 L167.69,400.11 L150,400.11 L150,268.05 Z M99.33,268.05 L117,268.05 L117,400.14 L99.28,400.14 L99.33,268.05 Z M99.33,221.45 L117,221.45 L117,251.87 L99.28,251.87 L99.33,221.45 Z M29.6,283.26 L0.46,283.26 L0.46,268.05 L29.6,268.05 L29.6,221.45 L47.34,221.45 L47.34,268.05 L80.53,268.05 L80.53,283.26 L47.34,283.26 L47.34,400.14 L29.6,400.14 L29.6,283.26 Z" id="Shape" /></g></g></svg>');

        cfgEditor.ui.registry.addButton('tinyMceAttributionButton', {
            tooltip: 'POWERED BY TINY',
            icon: 'tinymce-toolbar-attribution',
            onAction: function (_) {
                var win = window.open("https://www.tiny.cloud/", "_blank");
                win.focus();
            }
        });
    };

    if (editor.dataset.enableFormatting === "False") {
        config.toolbar = false;
        config.menubar = false;
    }

    tinymce.init(config);
}