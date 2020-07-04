namespace SharpFPDF
{
    /// <summary>
    /// Possible zoom modes.
    /// </summary>
    public enum ZoomMode
    {
        /// <summary>
        ///  Displays the entire page on screen.
        /// </summary>
        Fullpage,

        /// <summary>
        /// Uses maximum width of window.
        /// </summary>
        Fullwidth,

        /// <summary>
        /// Uses real size (equivalent to 100% zoom).
        /// </summary>
        Real,

        /// <summary>
        /// Uses viewer default mode.
        /// </summary>
        Default
    }
}
