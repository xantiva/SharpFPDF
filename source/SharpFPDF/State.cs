namespace SharpFPDF
{
    /// <summary>
    /// Possible document states
    /// </summary>
    internal enum State
    {
        Init = 0,
        EndPage = 1,
        BeginPage = 2,
        Closed = 3
    }
}
