using AutoMapper;
using HospitalManagementSystem.Core.Models;
using HospitalManagementSystem.Core;
using HospitalManagementSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HospitalManagementSystem.Controllers
{
    public class PatientTreatmentsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PatientTreatmentsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var ptList = await _unitOfWork.Repository<PatientTreatment>().GetAllAsync(
                includeProperties: "Patient,Treatment");

            var viewModel = ptList.Select(pt => new PatientTreatmentViewModel
            {
                PatientId = pt.PatientId,
                PatientName = pt.Patient.PatientName,
                TreatmentId = pt.TreatmentId,
                TreatmentName = pt.Treatment.TreatmentName,
                ObtainDate = pt.ObtainDate,
                EndDate = pt.EndDate
            });

            return View(viewModel);
        }

        public async Task<IActionResult> Create()
        {
            var patients = await _unitOfWork.Repository<Patient>().GetAllAsync();
            var treatments = await _unitOfWork.Repository<Treatment>().GetAllAsync();

            var viewModel = new PatientTreatmentViewModel
            {
                Patients = patients.Select(p => new SelectListItem
                {
                    Value = p.PatientId.ToString(),
                    Text = p.PatientName
                }),
                Treatments = treatments.Select(t => new SelectListItem
                {
                    Value = t.TreatmentId.ToString(),
                    Text = t.TreatmentName
                }),
                ObtainDate = DateOnly.FromDateTime(DateTime.Today) // default to today
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(PatientTreatmentViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                // Reload dropdowns
                var patients = await _unitOfWork.Repository<Patient>().GetAllAsync();
                var treatments = await _unitOfWork.Repository<Treatment>().GetAllAsync();
                viewModel.Patients = patients.Select(p => new SelectListItem { Value = p.PatientId.ToString(), Text = p.PatientName });
                viewModel.Treatments = treatments.Select(t => new SelectListItem { Value = t.TreatmentId.ToString(), Text = t.TreatmentName });

                return View(viewModel);
            }

            var patientTreatment = new PatientTreatment
            {
                PatientId = viewModel.PatientId,
                TreatmentId = viewModel.TreatmentId,
                ObtainDate = viewModel.ObtainDate,
                EndDate = viewModel.EndDate
            };

            await _unitOfWork.Repository<PatientTreatment>().AddAsync(patientTreatment);
            await _unitOfWork.CompleteAsync();

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Details(int id)
        {
            var treatment = await _unitOfWork.Repository<PatientTreatment>().GetFirstOrDefaultAsync(
                filter: pt => pt.PatientId == id,
                includeProperties: "Patient,Treatment"
            );

            if (treatment == null) return NotFound();

            var viewModel = new PatientTreatmentViewModel
            {
                PatientId = treatment.PatientId,
                TreatmentId = treatment.TreatmentId,
                PatientName = treatment.Patient.PatientName,
                TreatmentName = treatment.Treatment.TreatmentName,
                ObtainDate = treatment.ObtainDate,
                EndDate = treatment.EndDate
            };

            return View(viewModel);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var treatment = await _unitOfWork.Repository<PatientTreatment>().GetFirstOrDefaultAsync(
                pt => pt.PatientId == id,
                includeProperties: "Patient,Treatment"
            );

            if (treatment == null) return NotFound();

            var viewModel = _mapper.Map<PatientTreatmentViewModel>(treatment);

            var patients = await _unitOfWork.Repository<Patient>().GetAllAsync();
            var treatments = await _unitOfWork.Repository<Treatment>().GetAllAsync();

            viewModel.Patients = patients.Select(p => new SelectListItem { Value = p.PatientId.ToString(), Text = p.PatientName });
            viewModel.Treatments = treatments.Select(t => new SelectListItem { Value = t.TreatmentId.ToString(), Text = t.TreatmentName });

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(PatientTreatmentViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                // Log validation errors
                foreach (var state in ModelState)
                {
                    foreach (var error in state.Value.Errors)
                    {
                        Console.WriteLine($"Key: {state.Key}, Error: {error.ErrorMessage}");
                    }
                }

                
                var patients = await _unitOfWork.Repository<Patient>().GetAllAsync();
                var treatments = await _unitOfWork.Repository<Treatment>().GetAllAsync();

                viewModel.Patients = patients.Select(p => new SelectListItem { Value = p.PatientId.ToString(), Text = p.PatientName });
                viewModel.Treatments = treatments.Select(t => new SelectListItem { Value = t.TreatmentId.ToString(), Text = t.TreatmentName });

                return View(viewModel);
            }

            
            var existingPatientTreatment = await _unitOfWork.Repository<PatientTreatment>().GetFirstOrDefaultAsync(
                pt => pt.PatientId == viewModel.PatientId && pt.TreatmentId == viewModel.TreatmentId,
                includeProperties: "Patient,Treatment");

            if (existingPatientTreatment == null)
            {
                return NotFound();
            }

            
            existingPatientTreatment.ObtainDate = viewModel.ObtainDate;
            existingPatientTreatment.EndDate = viewModel.EndDate;

           
            _unitOfWork.Repository<PatientTreatment>().Update(existingPatientTreatment);
            await _unitOfWork.CompleteAsync();

            return RedirectToAction(nameof(Index));
        }



        public async Task<IActionResult> Delete(int id)
        {
            var treatment = await _unitOfWork.Repository<PatientTreatment>().GetFirstOrDefaultAsync(
                filter: pt => pt.PatientId == id,
                includeProperties: "Patient,Treatment"
            );

            if (treatment == null) return NotFound();

            var viewModel = new PatientTreatmentViewModel
            {
                PatientId = treatment.PatientId,
                TreatmentId = treatment.TreatmentId,
                PatientName = treatment.Patient.PatientName,
                TreatmentName = treatment.Treatment.TreatmentName,
                ObtainDate = treatment.ObtainDate,
                EndDate = treatment.EndDate
            };

            return View(viewModel);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var treatment = await _unitOfWork.Repository<PatientTreatment>().GetFirstOrDefaultAsync(pt => pt.PatientId == id);
            if (treatment == null) return NotFound();

            _unitOfWork.Repository<PatientTreatment>().Delete(treatment);
            await _unitOfWork.CompleteAsync();

            return RedirectToAction(nameof(Index));
        }

    }

}
