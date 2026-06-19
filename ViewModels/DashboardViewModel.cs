using ClinicCare.Models;

namespace ClinicCare.ViewModels
{
    public class DashboardViewModel
    {
        public int TodayPatients { get; set; }

        public decimal TodayCollection { get; set; }

        public int TotalPatients { get; set; }

        public int TotalVisits { get; set; }

        public decimal TotalRevenue { get; set; }

        public decimal TotalExpense { get; set; }

        public decimal TotalProfit { get; set; }

        public decimal MonthRevenue { get; set; }

        public decimal MonthExpense { get; set; }

        public decimal MonthProfit { get; set; }

        public List<Medicine> LowStockMedicines { get; set; }
            = new();

        public List<Medicine> ExpiringMedicines { get; set; }
            = new();
    }
}