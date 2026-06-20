namespace ClinicCare.Models
{
    public class Appointment
    {
        public int Id { get; set; }

        public int PatientId { get; set; }

        public Patient Patient { get; set; }

        public DateTime AppointmentDate { get; set; }

        public string TimeSlot { get; set; } = string.Empty;

        public string Status { get; set; } = "Pending";

        public DateTime CreatedDate { get; set; }
            = DateTime.Now;
    }
}