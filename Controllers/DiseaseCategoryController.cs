using ClinicCare.Data;
using ClinicCare.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinicCare.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DiseaseCategoryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DiseaseCategoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var categories =
                _context.DiseaseCategories.ToList();

            return View(categories);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(DiseaseCategory category)
        {
            if (!ModelState.IsValid)
            {
                return View(category);
            }

            _context.DiseaseCategories.Add(category);

            _context.SaveChanges();

            TempData["Success"] =
                "Disease Category Added Successfully";

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var category = _context.DiseaseCategories
                .FirstOrDefault(x => x.Id == id);

            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        [HttpPost]
        public IActionResult Edit(DiseaseCategory category)
        {
            if (!ModelState.IsValid)
            {
                return View(category);
            }

            _context.DiseaseCategories.Update(category);

            _context.SaveChanges();

            TempData["Success"] =
                "Disease Category Updated Successfully";

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var category = _context.DiseaseCategories
                .FirstOrDefault(x => x.Id == id);

            if (category == null)
            {
                return NotFound();
            }

            _context.DiseaseCategories.Remove(category);

            _context.SaveChanges();

            TempData["Success"] =
                "Disease Category Deleted Successfully";

            return RedirectToAction(nameof(Index));
        }
    }
}