namespace ClinicCare.Models
{
    public class Medicine
    {
        public int Id { get; set; }

        public string MedicineName { get; set; } = string.Empty;

        public int MedicineCategoryId { get; set; }

        public int StockQuantity { get; set; }

        public DateTime ExpiryDate { get; set; }

        public decimal PurchasePrice { get; set; }

        public MedicineCategory? MedicineCategory { get; set; }
    }
}