namespace ClinicCare.Models
{
    public class MedicinePurchase
    {
        public int Id { get; set; }

        public int MedicineId { get; set; }

        public Medicine Medicine { get; set; }

        public int QuantityPurchased { get; set; }

        public decimal PurchaseAmount { get; set; }

        public string? InvoiceNumber { get; set; }

        public string? Notes { get; set; }

        public DateTime PurchaseDate { get; set; }
    }
}
