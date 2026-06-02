using ClinicCare.Data;
using ClinicCare.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinicCare.Controllers
{
    [Authorize(Roles = "Admin")]
    public class MedicineCategoryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MedicineCategoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var categories = _context.MedicineCategories.ToList();

            return View(categories);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(MedicineCategory category)
        {
            if (!ModelState.IsValid)
            {
                return View(category);
            }

            _context.MedicineCategories.Add(category);

            _context.SaveChanges();

            TempData["Success"] = "Category Added Successfully";

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var category = _context.MedicineCategories
                .FirstOrDefault(x => x.Id == id);

            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }
        [HttpPost]
        public IActionResult Edit(MedicineCategory category)
        {
            if (!ModelState.IsValid)
            {
                return View(category);
            }

            _context.MedicineCategories.Update(category);

            _context.SaveChanges();

            TempData["Success"] = "Category Updated Successfully";

            return RedirectToAction(nameof(Index));
        }

        //temp 
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var category = _context.MedicineCategories
                .FirstOrDefault(x => x.Id == id);

            if (category == null)
            {
                return NotFound();
            }

            _context.MedicineCategories.Remove(category);

            _context.SaveChanges();

            TempData["Success"] = "Category Deleted Successfully";

            return RedirectToAction(nameof(Index));
        }
    }
}