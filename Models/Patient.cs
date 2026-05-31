using ClinicCare.Models;

public class Patient
{
    public int Id { get; set; }

    public string PatientCode { get; set; } = string.Empty;

    public string FullName { get; set; } = string.Empty;

    public DateTime DateOfBirth { get; set; }

    public string Gender { get; set; } = string.Empty;

    public string MobileNumber { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
    = DateTime.Now;

    public ICollection<PatientVisit> Visits { get; set; }
        = new List<PatientVisit>();
}