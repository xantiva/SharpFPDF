using System;
using System.Collections.Generic;
using System.Text;

namespace SharpFPDF.Fonts
{
    public abstract class PdfFont
    {
        public string File{ get; protected set; } = String.Empty;
        public string Type { get; protected set; } = String.Empty;
        public string Name { get; protected set; } = String.Empty;
        public Dictionary<string, string> Desc { get; protected set; } = new Dictionary<string, string>();
        public double UnderlinePosition { get; protected set; }
        public double UnderlineThickness { get; protected set; }
        public Dictionary<char, double> CharacterWidths { get; private set; } = new Dictionary<char, double>();
        public string? Encoding { get; protected set; }
        public Dictionary<int, int[]> Uv { get; private set; } = new Dictionary<int, int[]>();
        public bool Subsetted { get; private set; }

        /// <summary>
        /// Index, will be set after loading the font.
        /// </summary>
        public int I { get; set; }

        /// <summary>
        /// ?? Used in PutFonts
        /// </summary>
        public int N { get; set; }

        public string? Diff { get; set; }
    }
}
