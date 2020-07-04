namespace SharpFPDF
{
    public sealed class MetaData
    {
        /// <summary>
        /// Title of document
        /// </summary>
        internal string Title { get; set; } = string.Empty;

        /// <summary>
        /// Author of document
        /// </summary>
        internal string Author { get; set; } = string.Empty;

        /// <summary>
        /// Subject of document
        /// </summary>
        internal string Subject { get; set; } = string.Empty;

        /// <summary>
        /// Keywords of document
        /// </summary>
        internal string Keywords { get; set; } = string.Empty;

        /// <summary>
        /// Creator of document
        /// </summary>
        internal string Creator { get; set; } = "SharpFPDF";
    }
}
