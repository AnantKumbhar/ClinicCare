namespace ClinicCare.ViewModels
{
    public class VisitDetailsViewModel
    {
        public long VisitNumber { get; set; }

        public string PatientCode { get; set; }

        public string PatientName { get; set; }

        public string Disease { get; set; }

        public decimal AmountPaid { get; set; }

        public DateTime VisitDate { get; set; }

        public string? Notes { get; set; }

        public List<VisitMedicineViewModel> Medicines { get; set; }
            = new();
    }
}