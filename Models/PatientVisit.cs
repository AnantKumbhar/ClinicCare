namespace ClinicCare.Models
{
    public class PatientVisit
    {
        public int Id { get; set; }

        public long VisitNumber { get; set; }

        public int PatientId { get; set; }

        public DateTime VisitDate { get; set; }

        public int DiseaseCategoryId { get; set; }

        public decimal AmountPaid { get; set; }

        public string Notes { get; set; } = string.Empty;
        public DiseaseCategory? DiseaseCategory { get; set; }

        public Patient? Patient { get; set; }
    }
}