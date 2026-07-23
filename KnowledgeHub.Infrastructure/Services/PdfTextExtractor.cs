using KnowledgeHub.Application.Interfaces;
using UglyToad.PdfPig;

namespace KnowledgeHub.Infrastructure.Services;

public class PdfTextExtractor : IPdfTextExtractor
{

    public string ExtractText(string pdfPath)
    {
        using var document = PdfDocument.Open(pdfPath);

        var text = string.Empty;

        foreach (var page in document.GetPages())
        {
            text += page.Text;
            text += Environment.NewLine;
        }

        return text;
    }
}
