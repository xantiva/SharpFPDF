namespace SharpFPDF
{
    public sealed class PageInfo
    {
        public Size? Size { get; set; } = null;
        public Rotation? Rotation { get; set; } = null;

        public int N { get; set; }
    }
}
