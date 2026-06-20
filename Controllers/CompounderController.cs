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
            return View();
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
