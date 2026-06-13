using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ClinicCare.ViewModels
{
    public class PatientEntryViewModel
    {
        public string? PatientCode { get; set; }

        
        public string FullName { get; set; } = string.Empty;

        [Required]
        public DateTime DateOfBirth { get; set; }

        
        public string Gender { get; set; } = string.Empty;

       
        public string MobileNumber { get; set; } = string.Empty;

        [Required]
        public int DiseaseCategoryId { get; set; }

        [Required]
        public decimal AmountPaid { get; set; }

        public string Notes { get; set; } = string.Empty;
        public List<SelectListItem> DiseaseCategories { get; set; } = new();

        public List<PatientVisitMedicineViewModel> Medicines { get; set; } = new();

        public List<SelectListItem> MedicineOptions { get; set; } = new();
    }
}