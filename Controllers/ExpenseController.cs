using ClinicCare.Data;
using ClinicCare.Models;
using ClinicCare.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ClinicCare.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ExpenseController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ExpenseController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var expenses = _context.Expenses
                .Select(e => new
                {
                    e.Id,
                    CategoryName = e.ExpenseCategory.Name,
                    e.Amount,
                    e.ExpenseDate,
                    e.Notes,
                    e.IsSystemGenerated
                })
                .OrderByDescending(x => x.ExpenseDate)
                .ToList();

            ViewBag.TotalExpense =
                _context.Expenses.Sum(x => x.Amount);

            ViewBag.MonthExpense =
                _context.Expenses
                .Where(x =>
                    x.ExpenseDate.Month == DateTime.Today.Month &&
                    x.ExpenseDate.Year == DateTime.Today.Year)
                .Sum(x => x.Amount);

            ViewBag.TodayExpense =
                _context.Expenses
                .Where(x =>
                    x.ExpenseDate.Date == DateTime.Today)
                .Sum(x => x.Amount);

            return View(expenses);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var model = new ExpenseViewModel();

            model.Categories = _context.ExpenseCategories
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                })
                .ToList();

            model.ExpenseDate = DateTime.Today;

            return View(model);
        }

        [HttpPost]
        public IActionResult Create(ExpenseViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Categories = _context.ExpenseCategories
                    .Select(c => new SelectListItem
                    {
                        Value = c.Id.ToString(),
                        Text = c.Name
                    })
                    .ToList();

                return View(model);
            }

            var expense = new Expense
            {
                ExpenseCategoryId = model.ExpenseCategoryId,
                Amount = model.Amount,
                ExpenseDate = model.ExpenseDate,
                Notes = model.Notes,
                IsSystemGenerated = false
            };

            _context.Expenses.Add(expense);

            _context.SaveChanges();

            TempData["Success"] =
                "Expense Added Successfully";

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var expense = _context.Expenses
                .FirstOrDefault(x => x.Id == id);

            if (expense == null)
            {
                return NotFound();
            }

            if (expense.IsSystemGenerated)
            {
                TempData["Error"] =
                    "System generated expenses cannot be edited.";

                return RedirectToAction(nameof(Index));
            }

            var model = new ExpenseViewModel
            {
                Id = expense.Id,
                ExpenseCategoryId = expense.ExpenseCategoryId,
                Amount = expense.Amount,
                ExpenseDate = expense.ExpenseDate,
                Notes = expense.Notes,

                Categories = _context.ExpenseCategories
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
        public IActionResult Edit(ExpenseViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Categories =
                    _context.ExpenseCategories
                    .Select(c => new SelectListItem
                    {
                        Value = c.Id.ToString(),
                        Text = c.Name
                    })
                    .ToList();

                return View(model);
            }

            var expense = _context.Expenses
                .FirstOrDefault(x => x.Id == model.Id);

            if (expense == null)
            {
                return NotFound();
            }

            expense.ExpenseCategoryId =
                model.ExpenseCategoryId;

            expense.Amount =
                model.Amount;

            expense.ExpenseDate =
                model.ExpenseDate;

            expense.Notes =
                model.Notes;

            _context.SaveChanges();

            TempData["Success"] =
                "Expense Updated Successfully";

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var expense = _context.Expenses
                .FirstOrDefault(x => x.Id == id);

            if (expense == null)
            {
                return NotFound();
            }
            if (expense.IsSystemGenerated)
{
    TempData["Error"] =
        "System generated expenses cannot be deleted.";

    return RedirectToAction(nameof(Index));
}

            _context.Expenses.Remove(expense);

            _context.SaveChanges();

            TempData["Success"] =
                "Expense Deleted Successfully";

            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public IActionResult Report(
    DateTime? fromDate,
    DateTime? toDate,
    int? expenseCategoryId)
        {
            var query = _context.Expenses.AsQueryable();

            if (fromDate.HasValue)
            {
                query = query.Where(x =>
                    x.ExpenseDate.Date >= fromDate.Value.Date);
            }

            if (toDate.HasValue)
            {
                query = query.Where(x =>
                    x.ExpenseDate.Date <= toDate.Value.Date);
            }

            if (expenseCategoryId.HasValue)
            {
                query = query.Where(x =>
                    x.ExpenseCategoryId ==
                    expenseCategoryId.Value);
            }

            var model = new ExpenseReportViewModel
            {
                FromDate = fromDate,

                ToDate = toDate,

                ExpenseCategoryId = expenseCategoryId,

                Categories = _context.ExpenseCategories
                    .Select(x => new SelectListItem
                    {
                        Value = x.Id.ToString(),
                        Text = x.Name
                    })
                    .ToList(),

                Expenses = query
                    .OrderByDescending(x => x.ExpenseDate)
                    .Select(x =>
                        new ExpenseReportItemViewModel
                        {
                            ExpenseDate = x.ExpenseDate,

                            CategoryName =
                                x.ExpenseCategory.Name,

                            Amount = x.Amount,

                            Notes = x.Notes
                        })
                    .ToList()
            };

            ViewBag.TotalAmount =
                model.Expenses.Sum(x => x.Amount);

            ViewBag.TotalRecords =
                model.Expenses.Count;

            return View(model);
        }
    }
}