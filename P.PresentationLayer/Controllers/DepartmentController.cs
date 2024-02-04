using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using P.BusinessLogicLayer.Interfaces;
using P.DataAccessLayer.Models;
using P.PresentationLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace P.PresentationLayer.Controllers
{
    [Authorize]
    public class DepartmentController : Controller
    {
        private readonly IUnitOfWork _UnitOfWork;
        private readonly IMapper _mapper;
        public DepartmentController(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _UnitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index()
        {
            var department= await _UnitOfWork.DepartmentRepository.GetAllAsync();
            var MappedDepartment = _mapper.Map<IEnumerable<Department>, IEnumerable<DepartmentViewModel>>(department);
            return View(MappedDepartment);
        }
        public IActionResult Create() 
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(DepartmentViewModel DepartmentVM)
        {
            if(ModelState.IsValid) 
            {
                var MappedDepartment = _mapper.Map<DepartmentViewModel, Department>(DepartmentVM);
                await _UnitOfWork.DepartmentRepository.AddAsync(MappedDepartment);
                
                int Result=await _UnitOfWork.CompleteAsync();
                if(Result>0)
                {
                    TempData["Message"] = "Department IS Created";
                }
                return RedirectToAction(nameof(Index));
            
            }
            return View(DepartmentVM);
        }
        public async Task<IActionResult> Details(int?id,string ViewName="Details")
        {
            if (id is null)
                return BadRequest();
            var department = await _UnitOfWork.DepartmentRepository.GetByIdAsync(id.Value);
            if(department is null)
                return NotFound();
            var MappedDepartment = _mapper.Map<Department, DepartmentViewModel>(department);
            return View(ViewName, MappedDepartment);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int?id) 
        {
            //if(id is null)
            //    return BadRequest();
            //var department = _departmentRepository.GetById(id.Value);
            //if(department is null)
            //    return NotFound();
            //return View(department);
            return await Details(id, "Edit");
        
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(DepartmentViewModel DepartmentVM, [FromRoute]int id)
        {
            if(id!=DepartmentVM.Id)
                return BadRequest();
            if(ModelState.IsValid)
            {
                try
                {
                    var MappedDepartment = _mapper.Map<DepartmentViewModel,Department >(DepartmentVM);
                    _UnitOfWork.DepartmentRepository.Update(MappedDepartment);
                    await _UnitOfWork.CompleteAsync();
                    return RedirectToAction(nameof(Index));

                }
                catch(System.Exception ex)
                { 
                    ModelState.AddModelError(string.Empty, ex.Message);
                
                }
            }
            return View(DepartmentVM);
            
        }
        public async Task<IActionResult> Delete(int? id)
        {
            return await Details(id, "Delete");
        }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(DepartmentViewModel DepartmentVM, [FromRoute]int id)
        {
            if(id!=DepartmentVM.Id)
                return BadRequest();
            try
            {
                var MappedDepartment = _mapper.Map<DepartmentViewModel, Department>(DepartmentVM);
                _UnitOfWork.DepartmentRepository.Delete(MappedDepartment);
                await _UnitOfWork.CompleteAsync();
                return RedirectToAction(nameof(Index));
            }
            catch(System.Exception ex)
            {
                ModelState.AddModelError(string.Empty,ex.Message);
                return View(DepartmentVM);
            }
        }
    }
}
