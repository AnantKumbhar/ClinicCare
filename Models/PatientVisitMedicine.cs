using ClinicCare.Models;

public class PatientVisitMedicine
{
    public int Id { get; set; }

    public int PatientVisitId { get; set; }

    public int MedicineId { get; set; }

    public int Quantity { get; set; }

    public PatientVisit? PatientVisit { get; set; }

    public Medicine? Medicine { get; set; }
}