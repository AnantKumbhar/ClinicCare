namespace ClinicCare.ViewModels
{
    public class VisitHistoryViewModel
    {
        public int VisitId { get; set; }

        public long VisitNumber { get; set; }

        public string PatientCode { get; set; }

        public string PatientName { get; set; }

        public string Disease { get; set; }

        public decimal AmountPaid { get; set; }

        public DateTime VisitDate { get; set; }
    }
}