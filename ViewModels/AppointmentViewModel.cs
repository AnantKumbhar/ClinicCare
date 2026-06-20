using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ClinicCare.ViewModels
{
    public class AppointmentViewModel
    {
        [Required]
        public DateTime AppointmentDate { get; set; }

        [Required]
        public string TimeSlot { get; set; } = string.Empty;

        public List<SelectListItem> Slots { get; set; }
            = new();
    }
}