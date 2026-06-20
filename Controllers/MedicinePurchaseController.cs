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

            _context.SaveChanges();

            var expenseCategory = _context.ExpenseCategories
                .FirstOrDefault(x => x.Name == "Medicine Purchase");

            if (expenseCategory != null)
            {
                var expense = new Expense
                {
                    ExpenseCategoryId = expenseCategory.Id,

                    Amount = model.PurchaseAmount,

                    ExpenseDate = DateTime.Now,

                    Notes = $"Medicine Purchase - {medicine.MedicineName}",

                    IsSystemGenerated = true,

                    MedicinePurchaseId = purchase.Id
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
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var purchase = _context.MedicinePurchases
                .FirstOrDefault(x => x.Id == id);

            if (purchase == null)
            {
                return NotFound();
            }

            var model = new MedicinePurchaseViewModel
            {
                Id = purchase.Id,

                MedicineId = purchase.MedicineId,

                QuantityPurchased =
                    purchase.QuantityPurchased,

                PurchaseAmount =
                    purchase.PurchaseAmount,

                InvoiceNumber =
                    purchase.InvoiceNumber,

                Notes =
                    purchase.Notes,

                Medicines = _context.Medicines
                    .OrderBy(x => x.MedicineName)
                    .Select(x => new SelectListItem
                    {
                        Value = x.Id.ToString(),
                        Text = x.MedicineName
                    })
                    .ToList()
            };

            return View(model);
        }
        [HttpPost]
        public IActionResult Edit(
    MedicinePurchaseViewModel model)
        {
            if (!ModelState.IsValid)
            {
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

            var purchase = _context.MedicinePurchases
                .FirstOrDefault(x => x.Id == model.Id);

            if (purchase == null)
            {
                return NotFound();
            }

            var medicine = _context.Medicines
                .FirstOrDefault(x =>
                    x.Id == purchase.MedicineId);

            if (medicine == null)
            {
                return NotFound();
            }

            // Remove old stock
            medicine.StockQuantity -=
                purchase.QuantityPurchased;

            // Add new stock
            medicine.StockQuantity +=
                model.QuantityPurchased;

            purchase.QuantityPurchased =
                model.QuantityPurchased;

            purchase.PurchaseAmount =
                model.PurchaseAmount;

            purchase.InvoiceNumber =
                model.InvoiceNumber;

            purchase.Notes =
                model.Notes;

            var expense = _context.Expenses
                .FirstOrDefault(x =>
                    x.MedicinePurchaseId ==
                    purchase.Id);

            if (expense != null)
            {
                expense.Amount =
                    model.PurchaseAmount;

                expense.Notes =
                    $"Medicine Purchase - {medicine.MedicineName}";
            }

            _context.SaveChanges();

            TempData["Success"] =
                "Purchase Updated Successfully";

            return RedirectToAction(nameof(History));
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var purchase = _context.MedicinePurchases
                .FirstOrDefault(x => x.Id == id);

            if (purchase == null)
            {
                return NotFound();
            }

            var medicine = _context.Medicines
                .FirstOrDefault(x =>
                    x.Id == purchase.MedicineId);

            if (medicine != null &&
        medicine.StockQuantity <
        purchase.QuantityPurchased)
            {
                TempData["Error"] =
                    "Cannot delete purchase because some stock has already been used.";

                return RedirectToAction(nameof(History));
            }

            if (medicine != null)
            {
                medicine.StockQuantity -=
                    purchase.QuantityPurchased;

                if (medicine.StockQuantity < 0)
                {
                    medicine.StockQuantity = 0;
                }
            }

            var expense = _context.Expenses
                .FirstOrDefault(x =>
                    x.MedicinePurchaseId ==
                    purchase.Id);

            if (expense != null)
            {
                _context.Expenses.Remove(expense);
            }

            _context.MedicinePurchases
                .Remove(purchase);

            _context.SaveChanges();

            TempData["Success"] =
                "Purchase Deleted Successfully";

            return RedirectToAction(nameof(History));
        }
    }
}
