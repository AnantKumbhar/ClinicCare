using ClinicCare.Data;
using ClinicCare.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClinicCare.Controllers
{
    [Authorize(Roles = "Compounder")]
    public class CompounderController : Controller
    {
        private readonly ApplicationDbContext _context;
        public CompounderController(ApplicationDbContext context)
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
        public IActionResult Appointments()
        {
            var appointments = _context.Appointments
                .OrderByDescending(x => x.AppointmentDate)
                .Select(x => new AppointmentListViewModel
                {
                    Id = x.Id,

                    PatientId = x.PatientId,

                    PatientCode =
                        x.Patient.PatientCode,

                    PatientName =
                        x.Patient.FullName,

                    AppointmentDate =
                        x.AppointmentDate,

                    TimeSlot =
                        x.TimeSlot,

                    Status =
                        x.Status
                })
                .ToList();

            return View(appointments);
        }
        [HttpPost]
        public IActionResult ApproveAppointment(int id)
        {
            var appointment = _context.Appointments
                .FirstOrDefault(x => x.Id == id);

            if (appointment == null)
            {
                return NotFound();
            }

            appointment.Status = "Approved";

            _context.SaveChanges();

            TempData["Success"] =
                "Appointment Approved";

            return RedirectToAction(nameof(Appointments));
        }
        [HttpPost]
        public IActionResult DeclineAppointment(int id)
        {
            var appointment = _context.Appointments
                .FirstOrDefault(x => x.Id == id);

            if (appointment == null)
            {
                return NotFound();
            }

            appointment.Status = "Declined";

            _context.SaveChanges();

            TempData["Success"] =
                "Appointment Declined";

            return RedirectToAction(nameof(Appointments));
        }
    }
}
