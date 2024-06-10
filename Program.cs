using Odeon.InvoiceApp;

Console.WriteLine("Hello, World!");

using (var db = new ApplicationDbContext())
{
    db.Database.EnsureCreated();
}

string pdfFilePath = "Invoice_10407.pdf";
var pdfInvoices = PdfHelper.ReadPdf(pdfFilePath);

List<InvoicePdf> unmatchedInvoices = new List<InvoicePdf>();
List<InvoicePdf> priceDifferentInvoices = new List<InvoicePdf>();
List<InvoicePdf> dublicateInvoices = new List<InvoicePdf>();
List<InvoicePdf> successInvoices = new List<InvoicePdf>();

using (var db = new ApplicationDbContext())
{
    foreach (var item in pdfInvoices)
    {
        var invoiceEntity = db.Invoices.Where(x => x.FlightDate == item.FlightDate && x.FlightNo == item.FlightNumber).FirstOrDefault();

        if (invoiceEntity != null)
        {
            if (invoiceEntity.InvoiceNumber != null)
            {
                dublicateInvoices.Add(item);
                continue;
            }

            if (invoiceEntity.Price != item.TotalAmount)
            {
                priceDifferentInvoices.Add(item);
                continue;
            }
            successInvoices.Add(item);

            invoiceEntity.InvoiceNumber = item.InvoiceNo;

            db.SaveChanges();
        }
        else
        {
            unmatchedInvoices.Add(item);
        }
    }
}

string assemblyPath = System.Reflection.Assembly.GetExecutingAssembly().Location;

// Get the directory of the assembly
string assemblyDirectory = System.IO.Path.GetDirectoryName(assemblyPath);

DateTime now = DateTime.UtcNow;

var unmatchedFileLocation = CsvHelper.ExportToCsv<InvoicePdf>(unmatchedInvoices, Path.Combine(assemblyDirectory, "UnMatched" + now.ToString("yyyyMMdd_HHmmss") + ".csv"));
var priceDifferentLocation = CsvHelper.ExportToCsv<InvoicePdf>(priceDifferentInvoices, Path.Combine(assemblyDirectory, "PriceDifferences" + now.ToString("yyyyMMdd_HHmmss") + ".csv"));
var dublicateLocation = CsvHelper.ExportToCsv<InvoicePdf>(dublicateInvoices, Path.Combine(assemblyDirectory, "Dublicate" + now.ToString("yyyyMMdd_HHmmss") + ".csv"));


var smtpService = new SmtpService("smtp.zoho.com", 587, "", "");

var keyValuePairs = new[]
        {
            new Odeon.InvoiceApp.KeyValuePair("Un-Matched", unmatchedInvoices.Count().ToString()),
            new Odeon.InvoiceApp.KeyValuePair("Price Differences", priceDifferentInvoices.Count().ToString()),
            new Odeon.InvoiceApp.KeyValuePair("Dublicate", dublicateInvoices.Count().ToString()),
            new Odeon.InvoiceApp.KeyValuePair("Success", successInvoices.Count().ToString())
        };

// Generate HTML table
string emailBody = HtmlHelper.GenerateHtmlTable(keyValuePairs);


// Send email with CSV attachment
await smtpService.SendEmailAsync("seckin.celk@hotmail.com", "Invoice Checker", emailBody, new string[] { unmatchedFileLocation, priceDifferentLocation, dublicateLocation });