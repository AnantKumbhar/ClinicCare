namespace ClinicCare.ViewModels
{
    public class AppointmentListViewModel
    {
        public int Id { get; set; }

        public int PatientId { get; set; }

        public string PatientCode { get; set; }
            = string.Empty;

        public string PatientName { get; set; }
            = string.Empty;

        public DateTime AppointmentDate { get; set; }

        public string TimeSlot { get; set; }
            = string.Empty;

        public string Status { get; set; }
            = string.Empty;
    }
}