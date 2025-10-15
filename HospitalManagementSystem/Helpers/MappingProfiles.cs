using AutoMapper;
using HospitalManagementSystem.Core.Models;
using HospitalManagementSystem.ViewModels;

namespace HospitalManagementSystem.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            #region Doctor
            CreateMap<Doctor, DoctorViewModel>()
                .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department!.DepartmentName));

            CreateMap<DoctorViewModel, Doctor>();

            #endregion
            #region Deparmtment

            CreateMap<Department, DepartmentViewModel>().ReverseMap();

            #endregion
            #region Patient
            CreateMap<Patient, PatientViewModel>()
                .ForMember(dest => dest.RoomNumber, opt => opt.MapFrom(src => src.Room.RoomNumber));

            CreateMap<PatientViewModel, Patient>();

            #endregion
            #region Room
            CreateMap<Room, RoomViewModel>().ReverseMap();
            #endregion
            #region Treatment
            CreateMap<Treatment, TreatmentViewModel>().ReverseMap();
            #endregion
            #region PatientTreatment
            CreateMap<PatientTreatment, PatientTreatmentViewModel>()
                .ForMember(dest => dest.PatientName, opt => opt.MapFrom(src => src.Patient.PatientName))
                .ForMember(dest => dest.TreatmentName, opt => opt.MapFrom(src => src.Treatment.TreatmentName))
                .ForMember(dest => dest.Patients, opt => opt.Ignore())
                .ForMember(dest => dest.Treatments, opt => opt.Ignore());

            CreateMap<PatientTreatmentViewModel, PatientTreatment>();
            #endregion
            #region Appointment
            CreateMap<Appointment, AppointmentViewModel>()
                .ForMember(dest => dest.DoctorName, opt => opt.MapFrom(src => src.Doctor != null ? src.Doctor.DoctorName : string.Empty))
                .ForMember(dest => dest.PatientName, opt => opt.MapFrom(src => src.Patient != null ? src.Patient.PatientName : string.Empty))
                .ForMember(dest => dest.AppointmentTime, opt => opt.MapFrom(src => src.DateTime))
                .ReverseMap()
                .ForMember(dest => dest.Doctor, opt => opt.Ignore())
                .ForMember(dest => dest.Patient, opt => opt.Ignore())
                .ForMember(dest => dest.DateTime,opt => opt.MapFrom(src => src.AppointmentTime));


            #endregion
        }
    }
}
