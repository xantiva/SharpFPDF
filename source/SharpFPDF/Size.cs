using System;

namespace SharpFPDF
{
    public sealed class Size
    {
        public double Width { get; private set; }
        public double Height { get; private set; }

        public Size(double width, double heigth)
        {
            if (width <= 0.0 || heigth <= 0.0)
            {
                throw new ArgumentOutOfRangeException(
                    $"The width and height must be positive and not zero or negative. width: {width}, heigth: {heigth}");
            }

            if (width < heigth)
            {
                Width = width;
                Height = heigth;
            }
            else
            {
                Width = heigth;
                Height = width;
            }
        }
    }
}
