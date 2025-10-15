using AutoMapper;
using HospitalManagementSystem.Core.Models;
using HospitalManagementSystem.Core;
using HospitalManagementSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagementSystem.Controllers
{
    public class DepartmentsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DepartmentsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // GET: Departments
        public async Task<IActionResult> Index()
        {
            var departments = await _unitOfWork.Repository<Department>().GetAllAsync();
            var departmentVMs = _mapper.Map<IEnumerable<DepartmentViewModel>>(departments);
            return View(departmentVMs);
        }

        // GET: Departments/Create
        public IActionResult Create() => View();

        // POST: Departments/Create
        [HttpPost]
        public async Task<IActionResult> Create(DepartmentViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var newDepartment = new Department
            {
                DepartmentName = model.DepartmentName,
            };
            await _unitOfWork.Repository<Department>().AddAsync(newDepartment);
            await _unitOfWork.CompleteAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Departments/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var department = await _unitOfWork.Repository<Department>().GetByIdAsync(id);
            if (department == null) return NotFound();

            var model = _mapper.Map<DepartmentViewModel>(department);
            return View(model);
        }

        // POST: Departments/Edit/5
        [HttpPost]
        public async Task<IActionResult> Edit(int id, DepartmentViewModel model)
        {
            if (id != model.DepartmentId) return BadRequest();

            if (!ModelState.IsValid) return View(model);

            var department = await _unitOfWork.Repository<Department>().GetByIdAsync(id);
            if (department == null) return NotFound();

            _mapper.Map(model, department);
            _unitOfWork.Repository<Department>().Update(department);
            await _unitOfWork.CompleteAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Departments/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var department = await _unitOfWork.Repository<Department>().GetByIdAsync(id);
            if (department == null) return NotFound();

            _unitOfWork.Repository<Department>().Delete(department);
            var model = _mapper.Map<DepartmentViewModel>(department);
            return View(model);
        }

        // POST: Departments/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var department = await _unitOfWork.Repository<Department>().GetByIdAsync(id);
            if (department == null) return NotFound();

            _unitOfWork.Repository<Department>().Delete(department);
            await _unitOfWork.CompleteAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Departments/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var department = await _unitOfWork.Repository<Department>().GetByIdAsync(id);
            if (department == null) return NotFound();

            var model = _mapper.Map<DepartmentViewModel>(department);
            return View(model);
        }
    }
}
