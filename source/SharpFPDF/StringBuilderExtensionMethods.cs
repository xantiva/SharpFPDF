using System;
using System.Text;

namespace SharpFPDF
{
    public static class StringBuilderExtensionMethods
    {
        private const string pdfNewLine = "\n";
        public static StringBuilder AppendNewLine(this StringBuilder sb, string s)
        {
            if (sb == null) throw new ArgumentNullException(nameof(sb), "The string builder must not be null.");

            sb.Append(s);
            sb.Append(pdfNewLine);

            return sb;
        }
    }
}
