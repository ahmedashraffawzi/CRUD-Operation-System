using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using P.BusinessLogicLayer.Interfaces;
using P.BusinessLogicLayer.Repositories;
using P.DataAccessLayer.Models;
using P.PresentationLayer.Helpers;
using P.PresentationLayer.ViewModels;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace P.PresentationLayer.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        
        private readonly IMapper _mapper;
        public EmployeeController(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index(string SearchValue)
        {
            IEnumerable<Employee> employees;

            if(string.IsNullOrEmpty(SearchValue))
            {
                 employees =await _unitOfWork.EmployeeRepository.GetAllAsync();
               
            }
            else
            {
                 employees = _unitOfWork.EmployeeRepository.SearchEmployeesByName(SearchValue);
                
            }
               var MappedEmployee = _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeViewModel>>(employees);
               return View(MappedEmployee);

        }
        public IActionResult Create()
        {
            //ViewBag.Departments=_departmentRepository.GetAll();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(EmployeeViewModel EmployeeVM)
        {
            if (ModelState.IsValid)
            {
                EmployeeVM.ImageName= DocumentSettings.UploadFile(EmployeeVM.Image, "Image");
                var MappedEmployee=_mapper.Map<EmployeeViewModel,Employee>(EmployeeVM);
                await _unitOfWork.EmployeeRepository.AddAsync(MappedEmployee);
                
                await _unitOfWork.CompleteAsync();
                return RedirectToAction(nameof(Index));

            }
            return View(EmployeeVM);
        }
        public async Task<IActionResult> Details(int? id, string ViewName = "Details")
        {
            if (id is null)
                return BadRequest();
            var employee =await _unitOfWork.EmployeeRepository.GetByIdAsync(id.Value);
            if (employee is null)
                return NotFound();
            var MappedEmployee=_mapper.Map<Employee,EmployeeViewModel>(employee);
            return View(ViewName, MappedEmployee);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            
            return await Details(id, "Edit");

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EmployeeViewModel EmployeeVM, [FromRoute] int id)
        {
            if (id != EmployeeVM.Id)
                return BadRequest();
            if (ModelState.IsValid)
            {
                try
                {
                    EmployeeVM.ImageName = DocumentSettings.UploadFile(EmployeeVM.Image, "Image");
                    var MappedEmployee=_mapper.Map<EmployeeViewModel, Employee>(EmployeeVM);
                    _unitOfWork.EmployeeRepository.Update(MappedEmployee);
                    await _unitOfWork.CompleteAsync();
                    return RedirectToAction(nameof(Index));

                }
                catch (System.Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);

                }
            }
            return View(EmployeeVM);

        }
        public async Task<IActionResult> Delete(int? id)
        {
            return await Details(id, "Delete");
        }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(EmployeeViewModel EmployeeVM, [FromRoute] int id)
        {
            if (id != EmployeeVM.Id)
                return BadRequest();
            try
            {
                var MappedEmployee = _mapper.Map<EmployeeViewModel, Employee>(EmployeeVM);
                _unitOfWork.EmployeeRepository.Delete(MappedEmployee);
                int Result=await _unitOfWork.CompleteAsync();
                if(Result>0)
                {
                    DocumentSettings.DeleteFile(EmployeeVM.ImageName, "Image");
                }
                return RedirectToAction(nameof(Index));
            }
            catch (System.Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(EmployeeVM);
            }
        }
    }
}
