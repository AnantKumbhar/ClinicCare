using ClinicCare.Data;
using ClinicCare.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        public IActionResult VisitHistory()
        {
            var patientId = Convert.ToInt32(
                User.FindFirst("PatientId")?.Value);

            var visits = _context.PatientVisits
                .Where(v => v.PatientId == patientId)
                .OrderByDescending(v => v.VisitDate)
                .Select(v => new VisitHistoryViewModel
                {
                    VisitId = v.Id,

                    VisitNumber = v.VisitNumber,

                    PatientCode = v.Patient.PatientCode,

                    PatientName = v.Patient.FullName,

                    Disease = v.DiseaseCategory.Name,

                    AmountPaid = v.AmountPaid,

                    VisitDate = v.VisitDate
                })
                .ToList();

            return View(visits);
        }
        public IActionResult VisitDetails(int id)
        {
            var patientId = Convert.ToInt32(
                User.FindFirst("PatientId")?.Value);

            var visit = _context.PatientVisits
    .Include(v => v.Patient)
    .Include(v => v.DiseaseCategory)
    .FirstOrDefault(v =>
        v.Id == id &&
        v.PatientId == patientId);

            if (visit == null)
            {
                return NotFound();
            }

            var model = new VisitDetailsViewModel
            {
                VisitNumber = visit.VisitNumber,

                PatientCode =
                    visit.Patient.PatientCode,

                PatientName =
                    visit.Patient.FullName,

                Disease =
                    visit.DiseaseCategory.Name,

                AmountPaid =
                    visit.AmountPaid,

                VisitDate =
                    visit.VisitDate,

                Notes =
                    visit.Notes,

                Medicines =
                    _context.PatientVisitMedicines
                    .Where(x =>
                        x.PatientVisitId == id)
                    .Select(x =>
                        new VisitMedicineViewModel
                        {
                            MedicineName =
                                x.Medicine.MedicineName,

                            Quantity =
                                x.Quantity
                        })
                    .ToList()
            };

            return View(model);
        }
    }
}