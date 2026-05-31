namespace ClinicCare.ViewModels
{
    public class TodayEntriesViewModel
    {
        public long VisitNumber { get; set; }

        public string PatientCode { get; set; } = string.Empty;

        public string PatientName { get; set; } = string.Empty;

        public string Disease { get; set; } = string.Empty;

        public decimal AmountPaid { get; set; }

        public DateTime VisitDate { get; set; }
    }
}