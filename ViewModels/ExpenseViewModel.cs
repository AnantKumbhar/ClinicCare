using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ClinicCare.ViewModels
{
    public class ExpenseViewModel
    {
        public int Id { get; set; }

        [Required]
        public int ExpenseCategoryId { get; set; }

        [Required]
        [Range(0.01, 999999)]
        public decimal Amount { get; set; }

        [Required]
        public DateTime ExpenseDate { get; set; }

        public string? Notes { get; set; }

        public List<SelectListItem> Categories { get; set; }
            = new();
    }
}