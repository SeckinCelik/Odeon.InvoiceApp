using System;
namespace Odeon.InvoiceApp
{
    public class InvoiceEntity
    {
        public Guid Id { get; set; }
        public int BookingId { get; set; }
        public string Customer { get; set; }
        public string CarrierCode { get; set; }
        public int FlightNo { get; set; }
        public DateTime FlightDate { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public decimal Price { get; set; }
        public int? InvoiceNumber { get; set; }
    }
}
