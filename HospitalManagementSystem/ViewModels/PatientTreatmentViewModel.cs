using Microsoft.AspNetCore.Mvc.Rendering;

namespace HospitalManagementSystem.ViewModels
{
    public class PatientTreatmentViewModel
    {
        public int PatientId { get; set; }
        public string? PatientName { get; set; }

        public int TreatmentId { get; set; }
        public string? TreatmentName { get; set;}

        public DateOnly ObtainDate { get; set; }
        public DateOnly? EndDate { get; set; }

        public IEnumerable<SelectListItem>? Patients { get; set; }
        public IEnumerable<SelectListItem>? Treatments { get; set; }
    }
}
