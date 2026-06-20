using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ClinicCare.ViewModels
{
    public class PatientEntryViewModel
    {
        public int? AppointmentId { get; set; }
        public string? PatientCode { get; set; }

        [Required(ErrorMessage = "Full Name is required")]
        public string FullName { get; set; } = string.Empty;

        [Required]
        public DateTime DateOfBirth { get; set; }

        
        public string Gender { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mobile Number is required")]
        public string MobileNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Disease Category is required")]
        public int DiseaseCategoryId { get; set; }

        [Required(ErrorMessage = "Amount is required")]
        [Range(1, 100000)]
        public decimal AmountPaid { get; set; }

        public string Notes { get; set; } = string.Empty;
        public List<SelectListItem> DiseaseCategories { get; set; } = new();

        public List<PatientVisitMedicineViewModel> Medicines { get; set; } = new();

        public List<SelectListItem> MedicineOptions { get; set; } = new();
    }
}