using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odeon.InvoiceApp
{
    public static class HtmlHelper
    {
        public static string GenerateHtmlTable(KeyValuePair[] keyValuePairs)
        {
            // StringBuilder to build the HTML table
            var htmlBuilder = new StringBuilder();

            // Start table
            htmlBuilder.Append("<table border='1'>");

            // Add table header row
            htmlBuilder.Append("<tr><th>Key</th><th>Value</th></tr>");

            // Add table rows for key-value pairs
            foreach (var pair in keyValuePairs)
            {
                htmlBuilder.Append("<tr>");
                htmlBuilder.Append($"<td>{pair.Key}</td>");
                htmlBuilder.Append($"<td>{pair.Value}</td>");
                htmlBuilder.Append("</tr>");
            }

            // End table
            htmlBuilder.Append("</table>");

            return htmlBuilder.ToString();
        }
    }
}
