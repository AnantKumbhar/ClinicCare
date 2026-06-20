using ClinicCare.Data;
using ClinicCare.Models;
using ClinicCare.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

            ViewBag.Slots = GetSlots();

            ViewBag.RecentAppointments = _context.Appointments
                .Where(x => x.PatientId == patientId)
                .OrderByDescending(x => x.CreatedDate)
                .Take(5)
                .ToList();

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
        [HttpGet]
        public IActionResult BookAppointment()
        {
            var model = new AppointmentViewModel();

            model.AppointmentDate = DateTime.Today;

            model.Slots = GetSlots();

            return View(model);
        }

        [HttpPost]
        public IActionResult BookAppointment(
            AppointmentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Slots = GetSlots();

                return View(model);
            }

            var patientId = Convert.ToInt32(
                User.FindFirst("PatientId")?.Value);

            var appointment = new Appointment
            {
                PatientId = patientId,

                AppointmentDate =
                    model.AppointmentDate,

                TimeSlot =
                    model.TimeSlot,

                Status = "Pending"
            };

            _context.Appointments.Add(appointment);

            _context.SaveChanges();

            TempData["Success"] =
                "Appointment Booked Successfully";

            return RedirectToAction(nameof(Dashboard));
        }
        private List<SelectListItem> GetSlots()
        {
            return new List<SelectListItem>
    {
        new SelectListItem
        {
            Text="09:00 AM - 10:00 AM",
            Value="09:00 AM - 10:00 AM"
        },

        new SelectListItem
        {
            Text="10:00 AM - 11:00 AM",
            Value="10:00 AM - 11:00 AM"
        },

        new SelectListItem
        {
            Text="11:00 AM - 12:00 PM",
            Value="11:00 AM - 12:00 PM"
        },

        new SelectListItem
        {
            Text="12:00 PM - 01:00 PM",
            Value="12:00 PM - 01:00 PM"
        },

        new SelectListItem
        {
            Text="02:00 PM - 03:00 PM",
            Value="02:00 PM - 03:00 PM"
        },

        new SelectListItem
        {
            Text="03:00 PM - 04:00 PM",
            Value="03:00 PM - 04:00 PM"
        },

        new SelectListItem
        {
            Text="04:00 PM - 05:00 PM",
            Value="04:00 PM - 05:00 PM"
        }
    };
        }
        public IActionResult MyAppointments()
        {
            var patientId = Convert.ToInt32(
                User.FindFirst("PatientId")?.Value);

            var appointments = _context.Appointments
                .Where(x => x.PatientId == patientId)
                .OrderByDescending(x => x.AppointmentDate)
                .ToList();

            return View(appointments);
        }
    }
}