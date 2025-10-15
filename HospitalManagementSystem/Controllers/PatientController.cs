using AutoMapper;
using HospitalManagementSystem.Core.Models;
using HospitalManagementSystem.Core;
using HospitalManagementSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HospitalManagementSystem.Controllers
{
    public class PatientController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PatientController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var patients = await _unitOfWork.Repository<Patient>().GetAllAsync("Room");
            var viewModel = _mapper.Map<IEnumerable<PatientViewModel>>(patients);
            return View(viewModel);
        }

        public async Task<IActionResult> Create()
        {
            var rooms = await _unitOfWork.Repository<Room>().FindAsync(r => r.Occupied == false);
            ViewBag.Rooms = new SelectList(rooms, "RoomId", "RoomNumber");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(PatientViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var room = await _unitOfWork.Repository<Room>().GetByIdAsync(model.RoomId.Value);
            if (room == null) return NotFound();

            room.Occupied = true;
            _unitOfWork.Repository<Room>().Update(room);

            var patient = _mapper.Map<Patient>(model);
            patient.Room = room;
            patient.RoomId = room.RoomId;

            await _unitOfWork.Repository<Patient>().AddAsync(patient);
            await _unitOfWork.CompleteAsync();
            return RedirectToAction("Index");
        }


        public async Task<IActionResult> Edit(int id)
        {
            var patient = await _unitOfWork.Repository<Patient>().GetByIdAsync(id);
            if (patient == null) return NotFound();

            var rooms = await _unitOfWork.Repository<Room>().FindAsync(r => r.Occupied == false);
            ViewBag.Rooms = new SelectList(rooms, "RoomId", "RoomNumber", patient.RoomId);
            return View(_mapper.Map<PatientViewModel>(patient));
        }

        [HttpPost]
        public async Task<IActionResult> Edit(PatientViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var existingPatient = await _unitOfWork.Repository<Patient>().GetByIdAsync(model.PatientId);
            if (existingPatient == null) return NotFound();

            // If room changed, update old and new room states
            if (existingPatient.RoomId != model.RoomId)
            {
                var oldRoom = await _unitOfWork.Repository<Room>().GetByIdAsync(existingPatient.RoomId.Value);
                if (oldRoom != null)
                {
                    oldRoom.Occupied = false;
                    _unitOfWork.Repository<Room>().Update(oldRoom);
                }

                var newRoom = await _unitOfWork.Repository<Room>().GetByIdAsync(model.RoomId.Value);
                if (newRoom != null)
                {
                    newRoom.Occupied = true;
                    _unitOfWork.Repository<Room>().Update(newRoom);
                }
            }

            
            existingPatient.Room = await _unitOfWork.Repository<Room>().GetByIdAsync(model.RoomId.Value);
            _unitOfWork.Repository<Patient>().Update(existingPatient);
            await _unitOfWork.CompleteAsync();
            return RedirectToAction("Index");
        }


        public async Task<IActionResult> Details(int id)
        {
            var patient = await _unitOfWork.Repository<Patient>().GetByIdAsync(id, "Room");
            if (patient == null) return NotFound();
            return View(_mapper.Map<PatientViewModel>(patient));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var patient = await _unitOfWork.Repository<Patient>().GetByIdAsync(id);
            if (patient == null) return NotFound();
            return View(_mapper.Map<PatientViewModel>(patient));
        }

        [HttpPost,ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var patient = await _unitOfWork.Repository<Patient>().GetByIdAsync(id);
            if (patient == null) return NotFound();

            var room = await _unitOfWork.Repository<Room>().GetByIdAsync(patient.RoomId.Value);
            if (room != null)
            {
                room.Occupied = false;
                _unitOfWork.Repository<Room>().Update(room);
            }

            _unitOfWork.Repository<Patient>().Delete(patient);
            await _unitOfWork.CompleteAsync();
            return RedirectToAction("Index");
        }

    }
}
