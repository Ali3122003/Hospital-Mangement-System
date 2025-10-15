namespace HospitalManagementSystem.ViewModels
{
    public class PatientViewModel
    {
        public int PatientId { get; set; }
        public string PatientName { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public string? Phone { get; set; }

        public int? RoomId { get; set; }
        public string? RoomNumber { get; set; }
    }
}
