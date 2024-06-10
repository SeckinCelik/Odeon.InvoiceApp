using Microsoft.EntityFrameworkCore;

namespace Odeon.InvoiceApp
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<InvoiceEntity> Invoices { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite("Data Source=invoice.db");
    }
}
