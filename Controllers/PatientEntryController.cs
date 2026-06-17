using ClinicCare.Data;
using ClinicCare.Models;
using ClinicCare.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ClinicCare.Controllers
{
    [Authorize]
    public class PatientEntryController : Controller
    {
        [HttpGet]
        public IActionResult Create()
        {
            var model = new PatientEntryViewModel();

            model.DiseaseCategories =
                _context.DiseaseCategories
                .Select(d => new SelectListItem
                {
                    Value = d.Id.ToString(),
                    Text = d.Name
                })
                .ToList();

            model.MedicineOptions = _context.Medicines
    .OrderBy(x => x.MedicineName)
    .Select(m => new SelectListItem
    {
        Value = m.Id.ToString(),
        Text = m.MedicineName
    })
    .ToList();

            return View(model);
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
                model.DiseaseCategories =
        _context.DiseaseCategories
        .Select(d => new SelectListItem
        {
            Value = d.Id.ToString(),
            Text = d.Name
        })
        .ToList();

                model.MedicineOptions =
                    _context.Medicines
                    .Select(m => new SelectListItem
                    {
                        Value = m.Id.ToString(),
                        Text = m.MedicineName
                    })
                    .ToList();

                return View(model);
            }

            foreach (var item in model.Medicines)
            {
                var medicine = _context.Medicines
                    .FirstOrDefault(m => m.Id == item.MedicineId);

                if (medicine == null)
                {
                    continue;
                }

                if (item.Quantity > medicine.StockQuantity)
                {
                    TempData["Error"] =
                        $"Insufficient stock for {medicine.MedicineName}. Available: {medicine.StockQuantity}";

                    return RedirectToAction(nameof(Create));
                }
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
                DiseaseCategoryId = model.DiseaseCategoryId,
                AmountPaid = model.AmountPaid,
                Notes = model.Notes
            };

            _context.PatientVisits.Add(visit);
            _context.SaveChanges();

            foreach (var item in model.Medicines)
            {
                var medicine = _context.Medicines
                    .FirstOrDefault(m => m.Id == item.MedicineId);

                if (medicine == null)
                {
                    continue;
                }

                

                medicine.StockQuantity -= item.Quantity;

                var visitMedicine =
                    new PatientVisitMedicine
                    {
                        PatientVisitId = visit.Id,
                        MedicineId = item.MedicineId,
                        Quantity = item.Quantity
                    };

                _context.PatientVisitMedicines
                    .Add(visitMedicine);
            }

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
                    Disease = v.DiseaseCategory.Name,
                    AmountPaid = v.AmountPaid,
                    VisitDate = v.VisitDate
                })
                .ToList();

            ViewBag.TotalPatients = entries.Count;

            ViewBag.TotalCollection =
                entries.Sum(x => x.AmountPaid);

            return View(entries);
        }
        public IActionResult VisitHistory()
        {
            var visits = _context.PatientVisits
                .OrderByDescending(v => v.VisitDate)
                .Select(v => new VisitHistoryViewModel
                {
                    VisitId = v.Id,
                    VisitNumber = v.VisitNumber,
                    PatientId = v.PatientId,
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
            var visit = _context.PatientVisits
                .Include(v => v.Patient)
                .Include(v => v.DiseaseCategory)
                .FirstOrDefault(v => v.Id == id);

            if (visit == null)
            {
                return NotFound();
            }

            var model = new VisitDetailsViewModel
            {
                VisitNumber = visit.VisitNumber,

                PatientCode = visit.Patient.PatientCode,

                PatientName = visit.Patient.FullName,

                Disease = visit.DiseaseCategory.Name,

                AmountPaid = visit.AmountPaid,

                VisitDate = visit.VisitDate,

                Notes = visit.Notes,

                Medicines = _context.PatientVisitMedicines
                    .Where(x => x.PatientVisitId == id)
                    .Select(x => new VisitMedicineViewModel
                    {
                        MedicineName = x.Medicine.MedicineName,

                        Quantity = x.Quantity
                    })
                    .ToList()
            };

            return View(model);
        }
        public IActionResult PatientProfile(int id)
        {
            var patient = _context.Patients
                .FirstOrDefault(p => p.Id == id);

            if (patient == null)
            {
                return NotFound();
            }

            var visits = _context.PatientVisits
                .Where(v => v.PatientId == patient.Id)
                .OrderByDescending(v => v.VisitDate)
                .Select(v => new VisitHistoryViewModel
                {
                    VisitId = v.Id,
                    VisitNumber = v.VisitNumber,
                    PatientCode = patient.PatientCode,
                    PatientName = patient.FullName,
                    Disease = v.DiseaseCategory.Name,
                    AmountPaid = v.AmountPaid,
                    VisitDate = v.VisitDate
                })
                .ToList();

            var model = new PatientProfileViewModel
            {
                PatientCode = patient.PatientCode,
                FullName = patient.FullName,
                Gender = patient.Gender,
                MobileNumber = patient.MobileNumber,
                DateOfBirth = patient.DateOfBirth,

                TotalVisits = visits.Count,

                LastVisitDate = visits
                    .FirstOrDefault()?.VisitDate,

                Visits = visits
            };

            return View(model);
        }
    }
}