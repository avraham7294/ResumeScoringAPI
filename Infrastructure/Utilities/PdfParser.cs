using System.IO;
using UglyToad.PdfPig;
using System.Linq;

namespace AIResumeScoringAPI.Infrastructure.Utilities
{
    /// <summary>
    /// Utility class for extracting text content from PDF files.
    /// </summary>
    public static class PdfParser
    {
        /// <summary>
        /// Extracts all text from a PDF file stream.
        /// </summary>
        /// <param name="pdfStream">The input stream containing the PDF file.</param>
        /// <returns>The extracted text content from the PDF.</returns>
        public static string ExtractTextFromPdf(Stream pdfStream)
        {
            using var document = PdfDocument.Open(pdfStream);

            // 🔸 Concatenate text from all pages
            var allText = string.Join("\n", document.GetPages().Select(p => p.Text));

            return allText;
        }
    }
}
