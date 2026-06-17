using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ClinicCare.ViewModels
{
    public class MedicineViewModel
    {
        public int Id { get; set; }

        [Required]
        public string MedicineName { get; set; } = string.Empty;

        [Required]
        public int MedicineCategoryId { get; set; }

        public int StockQuantity { get; set; }

        [Required]
        public DateTime ExpiryDate { get; set; }

        [Required]
        public decimal PurchasePrice { get; set; }

        public List<SelectListItem> Categories { get; set; }
            = new();
    }
}