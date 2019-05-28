using CMS.DocumentEngine;

namespace Models.InlineEditors
{
    /// <summary>
    /// View model for Image uploader editor.
    /// </summary>
    public class ImageUploaderEditorViewModel : InlineEditorViewModel
    {
        /// <summary>
        /// Image.
        /// </summary>
        public DocumentAttachment Image { get; set; }


        /// <summary>
        /// Indicates if the message should be positioned absolutely for empty image.
        /// </summary>
        public bool UseAbsolutePosition { get; set; }


        /// <summary>
        /// Position of the message in case of absolute position.
        /// </summary>
        public PanelPositionEnum MessagePosition { get; set; } = PanelPositionEnum.Center;
    }
}