using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinicCare.Controllers
{
    [Authorize(Roles = "Compounder")]
    public class CompounderController : Controller
    {
        public IActionResult Dashboard()
        {
            return View();
        }
    }
}
