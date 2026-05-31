namespace ClinicCare.Models
{
    public class PatientVisit
    {
        public int Id { get; set; }

        public long VisitNumber { get; set; }

        public int PatientId { get; set; }

        public DateTime VisitDate { get; set; }

        public string Disease { get; set; } = string.Empty;

        public decimal AmountPaid { get; set; }

        public string Notes { get; set; } = string.Empty;

        public Patient? Patient { get; set; }
    }
}