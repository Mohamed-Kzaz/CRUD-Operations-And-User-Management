using AutoMapper;
using Demo.BLL.Interfaces;
using Demo.BLL.Repositories;
using Demo.DAL.Contexts;
using Demo.DAL.Entities;
using DemoPL.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DemoPL.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        //private readonly IDepartmentRepository _departmentRepository;
        private readonly IMapper _mapper;
        public DepartmentController(
            //IDepartmentRepository departmentRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            // _departmentRepository = departmentRepository;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index()
        {
            var depaertments = await _unitOfWork.DepartmentRepository.GetAll(); //Model
            var mappedDepartment = _mapper.Map<IEnumerable<DepartmentViewModel>>(depaertments); //ViewModel
            return View(mappedDepartment);
        }

        [HttpGet] //=> This is Default is not important to write
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(DepartmentViewModel departmentViewModel)
        {
            if (ModelState.IsValid)
            {
                var mappedDepartment = _mapper.Map<Department>(departmentViewModel);
                await _unitOfWork.DepartmentRepository.Add(mappedDepartment);
                return RedirectToAction("Index");
            }
            return View(departmentViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id is null)
                return NotFound();

            var department = await _unitOfWork.DepartmentRepository.Get(id);
            var mappedDepartment = _mapper.Map<DepartmentViewModel>(department);


            if (department is null)
                return NotFound();

            return View(mappedDepartment);

        }

        public async Task<IActionResult> Update(int? id)
        {

            if (id is null)
                return NotFound();

            var department = await _unitOfWork.DepartmentRepository.Get(id);

            var mappedDepartment = _mapper.Map<DepartmentViewModel>(department);


            if (department is null)
                return NotFound();

            return View(mappedDepartment);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int? id, DepartmentViewModel departmentViewModel)
        {

            if (id != departmentViewModel.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var mappedDepartment = _mapper.Map<Department>(departmentViewModel);
                    await _unitOfWork.DepartmentRepository.Update(mappedDepartment);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    return View(departmentViewModel);
                }
            }

            return View(departmentViewModel);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null)
                return NotFound();

            var department = await _unitOfWork.DepartmentRepository.Get(id);

            if (department is null)
                return NotFound();

            await _unitOfWork.DepartmentRepository.Delete(department);
            return RedirectToAction("Index");
        }
    }
}
