using ClinicCare.Data;
using ClinicCare.Models;
using ClinicCare.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ClinicCare.Controllers
{
    public class PatientAccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PatientAccountController(
            ApplicationDbContext context)
        {
            _context = context;
        }

        private string GeneratePatientCode()
        {
            var count = _context.Patients.Count() + 1;

            return $"P{count:D4}";
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(
            PatientRegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var usernameExists =
                _context.PatientUsers
                .Any(x => x.Username == model.Username);

            if (usernameExists)
            {
                ViewBag.Error =
                    "Username already exists.";

                return View(model);
            }

            var patient = new Patient
            {
                PatientCode = GeneratePatientCode(),

                FullName = model.FullName,

                DateOfBirth = model.DateOfBirth,

                Gender = model.Gender,

                MobileNumber = model.MobileNumber
            };

            _context.Patients.Add(patient);

            _context.SaveChanges();

            var patientUser = new PatientUser
            {
                PatientId = patient.Id,

                Username = model.Username,

                PasswordHash = model.Password,

                CreatedOn = DateTime.Now,

                IsActive = true
            };

            _context.PatientUsers.Add(patientUser);

            _context.SaveChanges();

            TempData["Success"] =
                "Registration Successful. Please Login.";

            return RedirectToAction(nameof(Login));
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(
            PatientLoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = _context.PatientUsers
                .FirstOrDefault(x =>
                    x.Username == model.Username &&
                    x.PasswordHash == model.Password &&
                    x.IsActive);

            if (user == null)
            {
                ViewBag.Error =
                    "Invalid Username or Password";

                return View(model);
            }

            var claims = new List<Claim>
            {
                new Claim(
                    ClaimTypes.Name,
                    user.Username),

                new Claim(
                    ClaimTypes.Role,
                    "Patient"),

                new Claim(
                    "PatientId",
                    user.PatientId.ToString())
            };

            var identity =
                new ClaimsIdentity(
                    claims,
                    CookieAuthenticationDefaults
                    .AuthenticationScheme);

            var principal =
                new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults
                .AuthenticationScheme,
                principal);

            return RedirectToAction(
                "Dashboard",
                "Patient");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults
                .AuthenticationScheme);

            return RedirectToAction(nameof(Login));
        }
    }
}