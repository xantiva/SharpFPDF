using System.Text;

namespace SharpFPDF
{
    internal static class StringBuilderExtensionMethods
    {
        private const string pdfNewLine = "\n";
        public static StringBuilder AppendNewLine(this StringBuilder sb, string s)
        {
            sb.Append(s);
            sb.Append(pdfNewLine);
            return sb;
        }
    }
}
