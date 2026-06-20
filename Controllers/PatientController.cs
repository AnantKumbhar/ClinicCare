using ClinicCare.Data;
using ClinicCare.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinicCare.Controllers
{
    [Authorize(Roles = "Patient")]
    public class PatientController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PatientController(
            ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Dashboard()
        {
            var patientId = Convert.ToInt32(
                User.FindFirst("PatientId")?.Value);

            var patient = _context.Patients
                .FirstOrDefault(x => x.Id == patientId);

            if (patient == null)
            {
                return NotFound();
            }

            var totalVisits = _context.PatientVisits
                .Count(x => x.PatientId == patientId);

            ViewBag.PatientName =
                patient.FullName;

            ViewBag.TotalVisits =
                totalVisits;

            ViewBag.LastVisit =
                _context.PatientVisits
                .Where(x => x.PatientId == patientId)
                .OrderByDescending(x => x.VisitDate)
                .Select(x => x.VisitDate)
                .FirstOrDefault();

            return View();
        }
        public IActionResult Profile()
        {
            var patientId = Convert.ToInt32(
                User.FindFirst("PatientId")?.Value);

            var patient = _context.Patients
                .FirstOrDefault(x => x.Id == patientId);

            if (patient == null)
            {
                return NotFound();
            }

            var model = new PatientProfileViewModel
            {
                PatientCode = patient.PatientCode,

                FullName = patient.FullName,

                DateOfBirth = patient.DateOfBirth,

                Gender = patient.Gender,

                MobileNumber = patient.MobileNumber
            };

            return View(model);
        }
    }
}