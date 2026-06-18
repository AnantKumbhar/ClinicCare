using ClinicCare.Data;
using ClinicCare.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinicCare.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ExpenseCategoryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ExpenseCategoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var categories = _context.ExpenseCategories
                .OrderBy(x => x.Name)
                .ToList();

            return View(categories);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(ExpenseCategory model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            _context.ExpenseCategories.Add(model);

            _context.SaveChanges();

            TempData["Success"] =
                "Expense Category Added Successfully";

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var category = _context.ExpenseCategories
                .FirstOrDefault(x => x.Id == id);

            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        [HttpPost]
        public IActionResult Edit(ExpenseCategory model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var category = _context.ExpenseCategories
                .FirstOrDefault(x => x.Id == model.Id);

            if (category == null)
            {
                return NotFound();
            }

            category.Name = model.Name;

            _context.SaveChanges();

            TempData["Success"] =
                "Expense Category Updated Successfully";

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var category = _context.ExpenseCategories
                .FirstOrDefault(x => x.Id == id);

            if (category == null)
            {
                return NotFound();
            }

            var hasExpenses = _context.Expenses
                .Any(x => x.ExpenseCategoryId == id);

            if (hasExpenses)
            {
                TempData["Error"] =
                    "Cannot delete category because expenses exist.";

                return RedirectToAction(nameof(Index));
            }

            _context.ExpenseCategories.Remove(category);

            _context.SaveChanges();

            TempData["Success"] =
                "Expense Category Deleted Successfully";

            return RedirectToAction(nameof(Index));
        }
    }
}