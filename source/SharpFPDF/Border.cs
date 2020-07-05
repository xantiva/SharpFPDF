using System;

namespace SharpFPDF
{
    /// <summary>
    /// Possible borders of a cell.
    /// Do NOT combine <see cref="NoBorder"/> or <see cref="Frame"/> with any other.
    /// This will throw an exception.
    /// </summary>
    [Flags]
    public enum Borders
    {
        NoBorder = 0,
        Frame = 1,
        Left = 2,
        Top = 4,
        Right = 8,
        Bottom = 16
    }
}
