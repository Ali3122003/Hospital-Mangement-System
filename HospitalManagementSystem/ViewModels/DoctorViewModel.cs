using HospitalManagementSystem.Core.Models;

namespace HospitalManagementSystem.ViewModels
{
    public class DoctorViewModel
    {
        public int DoctorId { get; set; }
        public string DoctorName { get; set; }
        public string? Specialty { get; set; }

        public int? DepartmentId { get; set; }
        public string? DepartmentName { get; set; }
        public List<Department>? Departments { get; set; }
    }
}
