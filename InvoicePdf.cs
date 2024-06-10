using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odeon.InvoiceApp
{
    public class InvoicePdf
    {
        public DateTime FlightDate { get; set; }
        public int FlightNumber { get; set; }
        public int NumberOfSeatsSold { get; set; }
        public decimal Price { get; set; }
        public decimal TotalAmount { get; set; }
        public int InvoiceNo { get; set; }
    }
}
