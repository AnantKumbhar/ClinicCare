using ClinicCare.Models;

namespace ClinicCare.ViewModels
{
    public class DashboardViewModel
    {
        public int TodayPatients { get; set; }

        public decimal TodayCollection { get; set; }

        public int TotalPatients { get; set; }

        public int TotalVisits { get; set; }

        public List<Medicine> LowStockMedicines { get; set; }
    = new();

        public List<Medicine> ExpiringMedicines { get; set; }
    = new();
    }
}