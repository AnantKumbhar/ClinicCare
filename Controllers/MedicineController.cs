using ClinicCare.Data;
using ClinicCare.Models;
using ClinicCare.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ClinicCare.Controllers
{
    
    public class MedicineController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MedicineController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var medicines = _context.Medicines
                .Select(m => new
                {
                    m.Id,
                    m.MedicineName,
                    CategoryName = m.MedicineCategory.Name,
                    m.StockQuantity,
                    m.ExpiryDate,
                    m.PurchasePrice
                })
                .ToList();

            return View(medicines);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var model = new MedicineViewModel();

            model.Categories = _context.MedicineCategories
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                })
                .ToList();

            return View(model);
        }

        [HttpPost]
        public IActionResult Create(MedicineViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Categories = _context.MedicineCategories
                    .Select(c => new SelectListItem
                    {
                        Value = c.Id.ToString(),
                        Text = c.Name
                    })
                    .ToList();

                return View(model);
            }

            var medicine = new Medicine
            {
                MedicineName = model.MedicineName,
                MedicineCategoryId = model.MedicineCategoryId,
                StockQuantity = 0,
                ExpiryDate = model.ExpiryDate,
                PurchasePrice = model.PurchasePrice
            };

            _context.Medicines.Add(medicine);

            _context.SaveChanges();

            TempData["Success"] = "Medicine Added Successfully";

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var medicine = _context.Medicines
    .FirstOrDefault(x => x.Id == id);

            if (medicine == null)
            {
                return NotFound();
            }

            var model = new MedicineViewModel
            {
                Id = medicine.Id,
                MedicineName = medicine.MedicineName,
                MedicineCategoryId = medicine.MedicineCategoryId,
                StockQuantity = medicine.StockQuantity,
                ExpiryDate = medicine.ExpiryDate,
                PurchasePrice = medicine.PurchasePrice,

                Categories = _context.MedicineCategories
                    .Select(c => new SelectListItem
                    {
                        Value = c.Id.ToString(),
                        Text = c.Name
                    })
                    .ToList()
            };

            return View(model);
        }
        [HttpPost]
        public IActionResult Edit(MedicineViewModel medicine)
        {
            if (!ModelState.IsValid)
            {
                medicine.Categories = _context.MedicineCategories
        .Select(c => new SelectListItem
        {
            Value = c.Id.ToString(),
            Text = c.Name
        })
        .ToList();
                return View(medicine);
            }

            var existingMedicine = _context.Medicines
                .FirstOrDefault(x => x.Id == medicine.Id);

            if (existingMedicine == null)
            {
                return NotFound();
            }

            existingMedicine.MedicineName = medicine.MedicineName;
            existingMedicine.MedicineCategoryId = medicine.MedicineCategoryId;
            existingMedicine.StockQuantity = medicine.StockQuantity;
            existingMedicine.ExpiryDate = medicine.ExpiryDate;
            existingMedicine.PurchasePrice = medicine.PurchasePrice;


            _context.SaveChanges();

            TempData["Success"] = "Medicine Updated Successfully";

            return RedirectToAction(nameof(Index));
        }

        
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var medicine = _context.Medicines
                .FirstOrDefault(x => x.Id == id);

            if (medicine == null)
            {
                return NotFound();
            }

            _context.Medicines.Remove(medicine);

            _context.SaveChanges();

            TempData["Success"] = "medicine Deleted Successfully";

            return RedirectToAction(nameof(Index));
        }
    }
}