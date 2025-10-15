using HospitalManagementSystem.Core.Models;
using System.ComponentModel.DataAnnotations;

namespace HospitalManagementSystem.ViewModels
{
    public class AppointmentViewModel
    {
        public int AppointmentId { get; set; }

        public int DoctorId { get; set; }
        public string? DoctorName { get; set; }

        public int PatientId { get; set; }
        public string? PatientName { get; set; }

        public DateTime AppointmentTime { get; set; }
    }


}
