using ClinicCare.Data;
using ClinicCare.Models;
using ClinicCare.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinicCare.Controllers
{
    [Authorize]
    public class PatientEntryController : Controller
    {
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        private readonly ApplicationDbContext _context;
        private string GeneratePatientCode()
        {
            var count = _context.Patients.Count() + 1;

            return $"P{count:D4}";
        }

        private long GenerateVisitNumber()
        {
            return _context.PatientVisits.Count() + 1001;
        }

        public PatientEntryController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpPost]
        public IActionResult Create(PatientEntryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var patient = _context.Patients
    .FirstOrDefault(p => p.PatientCode == model.PatientCode);

            if (patient == null)
            {
                patient = new Patient
                {
                    PatientCode = GeneratePatientCode(),
                    FullName = model.FullName,
                    DateOfBirth = model.DateOfBirth,
                    Gender = model.Gender,
                    MobileNumber = model.MobileNumber
                };

                _context.Patients.Add(patient);
                _context.SaveChanges();
            }

            var visit = new PatientVisit
            {
                VisitNumber = GenerateVisitNumber(),
                PatientId = patient.Id,
                VisitDate = DateTime.Now,
                Disease = model.Disease,
                AmountPaid = model.AmountPaid,
                Notes = model.Notes
            };

            _context.PatientVisits.Add(visit);
            _context.SaveChanges();

            TempData["Success"] = "Patient Entry Saved Successfully";

            return RedirectToAction(nameof(Create));
        }
        [HttpGet]
        public JsonResult SearchPatients(string term)
        {
            var patients = _context.Patients
                .Where(p =>
                    p.PatientCode.Contains(term) ||
                    p.FullName.Contains(term) ||
                    p.MobileNumber.Contains(term))
                .Select(p => new
                {
                    p.PatientCode,
                    p.FullName,
                    p.MobileNumber
                })
                .Take(10)
                .ToList();

            return Json(patients);
        }
        [HttpGet]
        public JsonResult GetPatientDetails(string patientCode)
        {
            var patient = _context.Patients
                .Where(p => p.PatientCode == patientCode)
                .Select(p => new
                {
                    p.PatientCode,
                    p.FullName,
                    p.DateOfBirth,
                    p.Gender,
                    p.MobileNumber
                })
                .FirstOrDefault();

            return Json(patient);
        }
        public IActionResult Today()
        {
            var today = DateTime.Today;

            var entries = _context.PatientVisits
                .Where(v => v.VisitDate.Date == today)
                .Select(v => new TodayEntriesViewModel
                {
                    VisitNumber = v.VisitNumber,
                    PatientCode = v.Patient.PatientCode,
                    PatientName = v.Patient.FullName,
                    Disease = v.Disease,
                    AmountPaid = v.AmountPaid,
                    VisitDate = v.VisitDate
                })
                .ToList();

            ViewBag.TotalPatients = entries.Count;

            ViewBag.TotalCollection =
                entries.Sum(x => x.AmountPaid);

            return View(entries);
        }
    }
}