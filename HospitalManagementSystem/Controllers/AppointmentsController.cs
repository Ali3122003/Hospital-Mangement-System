using AutoMapper;
using HospitalManagementSystem.Core;
using HospitalManagementSystem.Core.Models;
using HospitalManagementSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagementSystem.Controllers
{
    public class AppointmentsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AppointmentsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var appointments = await _unitOfWork.Repository<Appointment>().GetAllAsync(includeProperties: "Doctor,Patient");
            var viewModels = _mapper.Map<IEnumerable<AppointmentViewModel>>(appointments);
            return View(viewModels);
        }

        public async Task<IActionResult> Create()
        {
            await PopulateDoctorAndPatientDropdowns();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(AppointmentViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                await PopulateDoctorAndPatientDropdowns();
                return View(viewModel);
            }

            var appointment = _mapper.Map<Appointment>(viewModel);

            // Check if the doctor is available
            var existingAppointments = await _unitOfWork.Repository<Appointment>().GetAllAsyncWithFilter(
                a => a.DoctorId == viewModel.DoctorId && a.DateTime == viewModel.AppointmentTime);

            if (existingAppointments.Any())
            {
                ModelState.AddModelError("", "The selected doctor is not available at that time.");
                await PopulateDoctorAndPatientDropdowns();
                return View(viewModel);
            }
            var patient = await _unitOfWork.Repository<Patient>().GetByIdAsync(viewModel.PatientId);
            var doctor = await _unitOfWork.Repository<Doctor>().GetByIdAsync(viewModel.DoctorId);
            appointment.Patient = patient;
            appointment.Doctor = doctor;
            await _unitOfWork.Repository<Appointment>().AddAsync(appointment);
            //await _unitOfWork.CompleteAsync();
            //return RedirectToAction(nameof(Index));
            try
            {
                await _unitOfWork.CompleteAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                var innerMessage = ex.InnerException?.Message;
                Console.WriteLine("EF Error: " + innerMessage);
                ModelState.AddModelError("", "Unable to save changes: " + innerMessage);
                await PopulateDoctorAndPatientDropdowns();
                return View(viewModel);
            }

        }

        public async Task<IActionResult> Edit(int id)
        {
            var appointment = await _unitOfWork.Repository<Appointment>().GetByIdAsync(id);
            if (appointment == null) return NotFound();

            var viewModel = _mapper.Map<AppointmentViewModel>(appointment);
            await PopulateDoctorAndPatientDropdowns();
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(AppointmentViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                await PopulateDoctorAndPatientDropdowns();
                return View(viewModel);
            }

            var appointment = _mapper.Map<Appointment>(viewModel);
            _unitOfWork.Repository<Appointment>().Update(appointment);
            await _unitOfWork.CompleteAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Details(int id)
        {
            // Get the appointment including related Doctor and Patient
            var appointment = await _unitOfWork.Repository<Appointment>()
                .GetAllAsync(includeProperties: "Doctor,Patient");

            var appointmentEntity = appointment.FirstOrDefault(a => a.AppointmentId == id);

            if (appointmentEntity == null)
                return NotFound();

            // Map to ViewModel
            var viewModel = _mapper.Map<AppointmentViewModel>(appointmentEntity);

            return View(viewModel);
        }


        public async Task<IActionResult> Delete(int id)
        {
            var appointment = await _unitOfWork.Repository<Appointment>()
                .GetAllAsync("Doctor,Patient");

            var appointmentEntity = appointment.FirstOrDefault(a => a.AppointmentId == id);
            if (appointmentEntity == null)
                return NotFound();

            var viewModel = _mapper.Map<AppointmentViewModel>(appointmentEntity);
            return View(viewModel);
        }

        // POST: Appointments/DeleteConfirmed/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var appointment = await _unitOfWork.Repository<Appointment>().GetByIdAsync(id);
            if (appointment == null)
                return NotFound();

            _unitOfWork.Repository<Appointment>().Delete(appointment);
            await _unitOfWork.CompleteAsync();
            return RedirectToAction(nameof(Index));
        }
        private async Task PopulateDoctorAndPatientDropdowns()
        {
            var doctors = await _unitOfWork.Repository<Doctor>().GetAllAsync();
            var patients = await _unitOfWork.Repository<Patient>().GetAllAsync();

            ViewBag.Doctors = new SelectList(doctors, "DoctorId", "DoctorName");
            ViewBag.Patients = new SelectList(patients, "PatientId", "PatientName");
        }
    }
}
