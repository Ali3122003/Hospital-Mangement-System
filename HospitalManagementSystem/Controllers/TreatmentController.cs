using AutoMapper;
using HospitalManagementSystem.Core.Models;
using HospitalManagementSystem.Core;
using HospitalManagementSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagementSystem.Controllers
{
    public class TreatmentsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TreatmentsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var treatments = await _unitOfWork.Repository<Treatment>().GetAllAsync();
            var viewModels = _mapper.Map<IEnumerable<TreatmentViewModel>>(treatments);
            return View(viewModels);
        }

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(TreatmentViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            var treatment = _mapper.Map<Treatment>(viewModel);
            await _unitOfWork.Repository<Treatment>().AddAsync(treatment);
            await _unitOfWork.CompleteAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var treatment = await _unitOfWork.Repository<Treatment>().GetByIdAsync(id);
            if (treatment == null) return NotFound();

            var viewModel = _mapper.Map<TreatmentViewModel>(treatment);
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(TreatmentViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            var treatment = _mapper.Map<Treatment>(viewModel);
            _unitOfWork.Repository<Treatment>().Update(treatment);
            await _unitOfWork.CompleteAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            var treatment = await _unitOfWork.Repository<Treatment>().GetByIdAsync(id);
            if (treatment == null) return NotFound();

            var viewModel = _mapper.Map<TreatmentViewModel>(treatment);
            return View(viewModel);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var treatment = await _unitOfWork.Repository<Treatment>().GetByIdAsync(id);
            if (treatment == null) return NotFound();

            var viewModel = _mapper.Map<TreatmentViewModel>(treatment);
            return View(viewModel);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var treatment = await _unitOfWork.Repository<Treatment>().GetByIdAsync(id);
            if (treatment == null) return NotFound();

            _unitOfWork.Repository<Treatment>().Delete(treatment);
            await _unitOfWork.CompleteAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
