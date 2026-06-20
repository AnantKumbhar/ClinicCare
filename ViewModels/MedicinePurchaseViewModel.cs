using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ClinicCare.ViewModels
{
    public class MedicinePurchaseViewModel
    {
        public int Id { get; set; }
        [Required]
        public int MedicineId { get; set; }

        [Required]
        [Range(1, 100000)]
        public int QuantityPurchased { get; set; }

        [Required]
        [Range(0.01, 1000000)]
        public decimal PurchaseAmount { get; set; }

        public string? InvoiceNumber { get; set; }

        public string? Notes { get; set; }

        public List<SelectListItem> Medicines { get; set; }
            = new();
    }
}