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
        NoBorder = 1,
        Frame = 2,
        Left = 4,
        Top = 8,
        Right = 16,
        Bottom = 32
    }
}
