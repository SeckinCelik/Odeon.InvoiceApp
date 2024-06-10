using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odeon.InvoiceApp
{
    public static class CsvHelper
    {
        public static string ExportToCsv<T>(IEnumerable<T> data, string filePath)
        {
            // Create CSV content
            var csvLines = data.Select(d => string.Join(",", d.GetType().GetProperties().Select(p => p.GetValue(d)?.ToString() ?? "")));

            // Write CSV content to file
            File.WriteAllLines(filePath, csvLines);
            
            return filePath;
        }
    }
}
