using AutoMapper;
using HospitalManagementSystem.Core.Models;
using HospitalManagementSystem.Core;
using HospitalManagementSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagementSystem.Controllers
{
    public class RoomsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RoomsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var rooms = await _unitOfWork.Repository<Room>().GetAllAsync();
            var viewModels = _mapper.Map<IEnumerable<RoomViewModel>>(rooms);
            return View(viewModels);
        }

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(RoomViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            var room = _mapper.Map<Room>(viewModel);
            await _unitOfWork.Repository<Room>().AddAsync(room);
            await _unitOfWork.CompleteAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var room = await _unitOfWork.Repository<Room>().GetByIdAsync(id);
            if (room == null) return NotFound();

            var viewModel = _mapper.Map<RoomViewModel>(room);
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(RoomViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            var room = _mapper.Map<Room>(viewModel);
            _unitOfWork.Repository<Room>().Update(room);
            await _unitOfWork.CompleteAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            var room = await _unitOfWork.Repository<Room>().GetByIdAsync(id);
            if (room == null) return NotFound();

            var viewModel = _mapper.Map<RoomViewModel>(room);
            return View(viewModel);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var room = await _unitOfWork.Repository<Room>().GetByIdAsync(id);
            if (room == null) return NotFound();

            var viewModel = _mapper.Map<RoomViewModel>(room);
            return View(viewModel);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var room = await _unitOfWork.Repository<Room>().GetByIdAsync(id);
            if (room == null) return NotFound();

            _unitOfWork.Repository<Room>().Delete(room);
            await _unitOfWork.CompleteAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
