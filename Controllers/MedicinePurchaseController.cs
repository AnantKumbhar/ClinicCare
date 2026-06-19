using ClinicCare.Data;
using ClinicCare.Models;
using ClinicCare.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ClinicCare.Controllers
{
    
    public class MedicinePurchaseController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MedicinePurchaseController(
            ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Create()
        {
            var model = new MedicinePurchaseViewModel();

            model.Medicines = _context.Medicines
                .OrderBy(x => x.MedicineName)
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.MedicineName
                })
                .ToList();

            return View(model);
        }
        [HttpPost]
        public IActionResult Create(
    MedicinePurchaseViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var medicine = _context.Medicines
                .FirstOrDefault(x =>
                    x.Id == model.MedicineId);

            if (medicine == null)
            {
                return NotFound();
            }

            medicine.StockQuantity +=
                model.QuantityPurchased;

            var purchase = new MedicinePurchase
            {
                MedicineId = model.MedicineId,

                QuantityPurchased =
                    model.QuantityPurchased,

                PurchaseAmount =
                    model.PurchaseAmount,

                InvoiceNumber =
                    model.InvoiceNumber,

                Notes =
                    model.Notes,

                PurchaseDate =
                    DateTime.Now
            };

            _context.MedicinePurchases
                .Add(purchase);

            var expenseCategory = _context.ExpenseCategories
                .FirstOrDefault(x => x.Name == "Medicine Purchase");

            if (expenseCategory != null)
            {
                var expense = new Expense
                {
                    ExpenseCategoryId = expenseCategory.Id,

                    Amount = model.PurchaseAmount,

                    ExpenseDate = DateTime.Now,

                    Notes = $"Medicine Purchase - {medicine.MedicineName}"
                };

                _context.Expenses.Add(expense);
            }

            _context.SaveChanges();

            TempData["Success"] =
                "Stock Added Successfully";

            return RedirectToAction(nameof(Create));
        }
        public IActionResult History()
        {
            var purchases = _context.MedicinePurchases
                .OrderByDescending(x => x.PurchaseDate)
                .Select(x => new MedicinePurchaseHistoryViewModel
                {
                    Id = x.Id,

                    MedicineName =
                        x.Medicine.MedicineName,

                    QuantityPurchased =
                        x.QuantityPurchased,

                    PurchaseAmount =
                        x.PurchaseAmount,

                    InvoiceNumber =
                        x.InvoiceNumber,

                    PurchaseDate =
                        x.PurchaseDate
                })
                .ToList();
            
                    ViewBag.TotalPurchases = purchases.Count;

                    ViewBag.TotalAmount = purchases.Sum(x => x.PurchaseAmount);

            return View(purchases);
        }
    }
}
