using ClinicCare.Data;
using ClinicCare.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClinicCare.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Dashboard()
        {
            var today = DateTime.Today;

            var model = new DashboardViewModel
            {
                TodayPatients = _context.PatientVisits
                    .Count(v => v.VisitDate.Date == today),

                TodayCollection = _context.PatientVisits
                    .Where(v => v.VisitDate.Date == today)
                    .Sum(v => (decimal?)v.AmountPaid) ?? 0,

                TotalPatients = _context.Patients.Count(),

                TotalVisits = _context.PatientVisits.Count(),

                TotalRevenue = _context.PatientVisits
    .Sum(x => (decimal?)x.AmountPaid) ?? 0,

                TotalExpense = _context.Expenses
    .Sum(x => (decimal?)x.Amount) ?? 0,

                TotalProfit =
(
    _context.PatientVisits
        .Sum(x => (decimal?)x.AmountPaid) ?? 0
)
-
(
    _context.Expenses
        .Sum(x => (decimal?)x.Amount) ?? 0
),

                MonthRevenue = _context.PatientVisits
    .Where(x =>
        x.VisitDate.Month == today.Month &&
        x.VisitDate.Year == today.Year)
    .Sum(x => (decimal?)x.AmountPaid) ?? 0,

                MonthExpense = _context.Expenses
    .Where(x =>
        x.ExpenseDate.Month == today.Month &&
        x.ExpenseDate.Year == today.Year)
    .Sum(x => (decimal?)x.Amount) ?? 0,

                MonthProfit =
(
    _context.PatientVisits
        .Where(x =>
            x.VisitDate.Month == today.Month &&
            x.VisitDate.Year == today.Year)
        .Sum(x => (decimal?)x.AmountPaid) ?? 0
)
-
(
    _context.Expenses
        .Where(x =>
            x.ExpenseDate.Month == today.Month &&
            x.ExpenseDate.Year == today.Year)
        .Sum(x => (decimal?)x.Amount) ?? 0
),

                LowStockMedicines = _context.Medicines
                    .Where(m => m.StockQuantity <= 10)
                    .OrderBy(m => m.StockQuantity)
                    .Take(10)
                    .ToList(),

                ExpiringMedicines = _context.Medicines
    .Where(m =>
        m.ExpiryDate <= DateTime.Today.AddDays(30))
    .OrderBy(m => m.ExpiryDate)
    .Take(10)
    .ToList()


            };

            return View(model);
        }
    }
}