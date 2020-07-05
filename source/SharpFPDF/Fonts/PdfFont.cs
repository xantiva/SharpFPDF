using System;
using System.Collections.Generic;
using System.Text;

namespace SharpFPDF.Fonts
{
    public abstract class PdfFont
    {
        public string Type { get; protected set; } = String.Empty;
        public string Name { get; protected set; } = String.Empty;
        public string Description { get; protected set; } = String.Empty;
        public double UnderlinePosition { get; protected set; }
        public double UnderlineThickness { get; protected set; }
        public Dictionary<char, double> CharacterWidths { get; private set; } = new Dictionary<char, double>();
        public Encoding Encoding { get; protected set; } = Encoding.Default;
        public Dictionary<int, int[]> Uw { get; private set; } = new Dictionary<int, int[]>();

        /// <summary>
        /// Index, will be set after loading the font.
        /// </summary>
        public int I { get; set; }
    }
}
