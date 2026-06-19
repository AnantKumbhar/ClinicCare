using Microsoft.AspNetCore.Mvc.Rendering;

namespace ClinicCare.ViewModels
{
    public class ExpenseReportViewModel
    {
        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        public int? ExpenseCategoryId { get; set; }

        public List<SelectListItem> Categories { get; set; }
            = new();

        public List<ExpenseReportItemViewModel> Expenses
            = new();
    }

    public class ExpenseReportItemViewModel
    {
        public DateTime ExpenseDate { get; set; }

        public string CategoryName { get; set; }
            = string.Empty;

        public decimal Amount { get; set; }

        public string? Notes { get; set; }
    }
}