namespace SharpFPDF
{
    /// <summary>
    /// Possible layout modes.
    /// </summary>
    public enum LayoutMode
    {
        /// <summary>
        /// Uses viewer default mode (= default value)
        /// </summary>
        Default,

        /// <summary>
        /// Displays one page at once.
        /// </summary>
#pragma warning disable CA1720 // Identifier contains type name
        Single,
#pragma warning restore CA1720 // Identifier contains type name

        /// <summary>
        /// Displays pages continuously.
        /// </summary>
        Continuous,

        /// <summary>
        /// Displays two pages on two columns.
        /// </summary>
        Two
    }
}
