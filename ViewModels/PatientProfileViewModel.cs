namespace ClinicCare.ViewModels
{
    public class PatientProfileViewModel
    {
        public string PatientCode { get; set; }

        public string FullName { get; set; }

        public string Gender { get; set; }

        public string MobileNumber { get; set; }

        public DateTime DateOfBirth { get; set; }

        public int TotalVisits { get; set; }

        public DateTime? LastVisitDate { get; set; }

        public List<VisitHistoryViewModel> Visits { get; set; }
            = new();
    }
}