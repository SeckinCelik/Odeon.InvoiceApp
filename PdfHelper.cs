using iText.Kernel.Pdf.Canvas.Parser.Listener;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf;

namespace Odeon.InvoiceApp
{
    public class PdfHelper
    {
        public static List<InvoicePdf> ReadPdf(string pdfFilePath)
        {
            List<InvoicePdf> invoicePdfs = new List<InvoicePdf>();

            if (File.Exists(pdfFilePath))
            {
                using (PdfReader pdfReader = new PdfReader(pdfFilePath))
                {
                    using (PdfDocument pdfDocument = new PdfDocument(pdfReader))
                    {
                        string extractedText = "";

                        for (int pageNumber = 1; pageNumber <= pdfDocument.GetNumberOfPages(); pageNumber++)
                        {
                            var page = pdfDocument.GetPage(pageNumber);
                            var strategy = new SimpleTextExtractionStrategy();
                            extractedText = PdfTextExtractor.GetTextFromPage(page, strategy);

                            var result = extractedText.Replace("Havacilik A.S\n", string.Empty).Split('\n');

                            var startIndex = Array.FindIndex(result, row => row == Constants.TotalAmount) + 2;
                            var endIndex = Array.FindIndex(result.Skip(startIndex).ToArray(), row => row.StartsWith(Constants.PageTotal));

                            if (endIndex == 0 || endIndex == -1)
                                endIndex = Array.FindIndex(result.Skip(startIndex).ToArray(), row => row.StartsWith(Constants.GrandTotal));

                            for (int i = startIndex; i < (startIndex + endIndex); i++)
                            {
                                var pdfItems = result[i].Split(' ', StringSplitOptions.RemoveEmptyEntries);

                                if (pdfItems[7].EndsWith('-'))
                                    continue;

                                DateTime parsedDate;
                                DateTime.TryParseExact(pdfItems[2], "dd.MM.yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out parsedDate);

                                invoicePdfs.Add(new InvoicePdf
                                {
                                    InvoiceNo = Convert.ToInt16(result[1]),
                                    FlightDate = parsedDate,
                                    FlightNumber = Convert.ToInt16(pdfItems[4]),
                                    Price = Convert.ToDecimal(pdfItems[8]),
                                    NumberOfSeatsSold = Convert.ToInt16(pdfItems[7]),
                                    TotalAmount = Convert.ToDecimal(pdfItems[9]),
                                });
                            }
                        }

                        return invoicePdfs;
                    }
                }
            }

            return invoicePdfs;
        }
    }
}
