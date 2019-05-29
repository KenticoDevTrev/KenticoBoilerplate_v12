namespace Models.InlineEditors
{
    /// <summary>
    /// View model for Text editor.
    /// </summary>
    public sealed class TinyMCEEditorViewModel : InlineEditorViewModel
    {
        /// <summary>
        /// HTML formatted text.
        /// </summary>
        public string Html { get; set; }


        /// <summary>
        /// Indicates if the formatting is enabled for the editor.
        /// </summary>
        public bool EnableFormatting { get; set; } = true;

        private string _Tools;

        public string Tools
        {
            get {
                if (!string.IsNullOrEmpty(_Tools) && !_Tools.EndsWith("tinyMceAttributionButton"))
                {
                    return _Tools + " | tinyMceAttributionButton";
                }

                return _Tools ?? "undo redo | formatselect | bold italic backcolor | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | removeformat | help fullscreen | tinyMceAttributionButton"; }
            set {
                

                _Tools = value; }
        }

        public string Plugins { get; set; } = "advlist autolink lists link image print preview anchor searchreplace visualblocks code fullscreen insertdatetime media table paste help wordcount";
    }
}