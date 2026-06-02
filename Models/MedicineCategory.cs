using System.ComponentModel.DataAnnotations;

namespace ClinicCare.Models
{
    public class MedicineCategory
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        public ICollection<Medicine> Medicines { get; set; }
            = new List<Medicine>();
    }
}