using System.IO;
using UglyToad.PdfPig;

namespace AIResumeScoringAPI.Infrastructure.Utilities
{
    public static class PdfParser
    {
        public static string ExtractTextFromPdf(Stream pdfStream)
        {
            using var document = PdfDocument.Open(pdfStream);
            return string.Join("\n", document.GetPages().Select(p => p.Text));
        }
    }
}
