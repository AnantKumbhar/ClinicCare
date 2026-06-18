using System.ComponentModel.DataAnnotations;

namespace ClinicCare.Models
{
    public class ExpenseCategory
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
    }
}