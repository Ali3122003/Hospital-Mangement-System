using AutoMapper;
using HospitalManagementSystem.Core.Models;
using HospitalManagementSystem.Core;
using HospitalManagementSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HospitalManagementSystem.Core.Specifications.DoctorWithDepartment.DoctorWithDepartmentSpec;

namespace HospitalManagementSystem.Controllers
{
    public class DoctorController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DoctorController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        
        public async Task<IActionResult> Index()
        {
            var doctors = await _unitOfWork.Repository<Doctor>().GetAllAsync(includeProperties: "Department");
            var vm = _mapper.Map<IEnumerable<DoctorViewModel>>(doctors);
            return View(vm);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var viewModel = new DoctorViewModel
            {
                Departments = (await _unitOfWork.Repository<Department>().GetAllAsync()).ToList()
            };
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(DoctorViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                vm.Departments = (await _unitOfWork.Repository<Department>().GetAllAsync()).ToList();
                foreach (var modelStateKey in ModelState.Keys)
                {
                    var value = ModelState[modelStateKey];
                    foreach (var error in value.Errors)
                    {
                        Console.WriteLine($"Key: {modelStateKey}, Error: {error.ErrorMessage}");
                    }
                }

                return View(vm);
            }

            var department = await _unitOfWork.Repository<Department>().GetByIdAsync(vm.DepartmentId.Value);

            if (department == null)
            {
                ModelState.AddModelError("DepartmentId", "Invalid Department");
                vm.Departments = (await _unitOfWork.Repository<Department>().GetAllAsync()).ToList();
                return View(vm);
            }
            var doctor = _mapper.Map<Doctor>(vm);

            doctor.Department = department;
            doctor.DepartmentId = department.DepartmentId;

            await _unitOfWork.Repository<Doctor>().AddAsync(doctor);
            await _unitOfWork.CompleteAsync();

            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var doctor = await _unitOfWork.Repository<Doctor>().GetByIdAsync(id);
            if (doctor == null)
                return NotFound();

            var vm = _mapper.Map<DoctorViewModel>(doctor);
            var departments = await _unitOfWork.Repository<Department>().GetAllAsync();
            ViewBag.Departments = departments;
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(DoctorViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var doctor = _mapper.Map<Doctor>(vm);
            _unitOfWork.Repository<Doctor>().Update(doctor);
            await _unitOfWork.CompleteAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var doctor = await _unitOfWork.Repository<Doctor>().GetByIdAsync(id);
            if (doctor == null)
                return NotFound();

            _unitOfWork.Repository<Doctor>().Delete(doctor);
            await _unitOfWork.CompleteAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            var doctor = await _unitOfWork.Repository<Doctor>().GetByIdAsync(id, includeProperties: "Department");
            if (doctor == null)
                return NotFound();

            var vm = _mapper.Map<DoctorViewModel>(doctor);
            return View(vm);
        }
    }

}
