namespace ClinicCare.ViewModels
{
    public class MedicinePurchaseHistoryViewModel
    {
        public int Id { get; set; }

        public string MedicineName { get; set; }

        public int QuantityPurchased { get; set; }

        public decimal PurchaseAmount { get; set; }

        public string? InvoiceNumber { get; set; }

        public DateTime PurchaseDate { get; set; }
    }
}