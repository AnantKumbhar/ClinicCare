using System.ComponentModel.DataAnnotations;

namespace ClinicCare.ViewModels
{
    public class PatientEntryViewModel
    {
        public string? PatientCode { get; set; }

        [Required]
        public string FullName { get; set; } = string.Empty;

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public string Gender { get; set; } = string.Empty;

        [Required]
        public string MobileNumber { get; set; } = string.Empty;

        [Required]
        public string Disease { get; set; } = string.Empty;

        [Required]
        public decimal AmountPaid { get; set; }

        public string Notes { get; set; } = string.Empty;
    }
}