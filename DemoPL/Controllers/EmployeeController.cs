using AutoMapper;
using Demo.BLL.Interfaces;
using Demo.BLL.Repositories;
using Demo.DAL.Entities;
using DemoPL.Helper;
using DemoPL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DemoPL.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        //private readonly IEmployeeRepository _employeeRepository;
        //private readonly IDepartmentRepository _departmentRepository;
        private readonly IMapper _mapper;

        public EmployeeController(
            //IEmployeeRepository employeeRepository,
            //IDepartmentRepository departmentRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            //_employeeRepository = employeeRepository;
            //_departmentRepository = departmentRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index(string SearchValue = "")
        {
            IEnumerable<Employee> employees;

            if(string.IsNullOrEmpty(SearchValue))
            {
                employees = await _unitOfWork.EmployeeRepository.GetAll(); //Model
            }
            else
            {
                employees = await _unitOfWork.EmployeeRepository.Search(SearchValue);
            }

            var mappedEmplyees = _mapper.Map<IEnumerable<EmployeeViewModel>>(employees); //ViewModel
            return View(mappedEmplyees);

        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Departments = await _unitOfWork.DepartmentRepository.GetAll();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(EmployeeViewModel employeeViewModel)
        {
            if (ModelState.IsValid)
            {
                //Manual Mapping => The Fastest
                //var mappedEmployee = new Employee()
                //{
                //    Id= employee.Id,
                //    Name= employee.Name,
                //    Address= employee.Address,
                //    Age= employee.Age,
                //    DepartmentId = employee.DepartmentId,
                //    Email = employee.Email,
                //    HireDate = employee.HireDate,
                //    IsActive = employee.IsActive,
                //    PhoneNumber = employee.PhoneNumber,
                //    Salary = employee.Salary,
                //};

                employeeViewModel.ImgUrl = DocumentSettings.UploadFile(employeeViewModel.Image, "Imgs");

                var mappedEmployee = _mapper.Map<Employee>(employeeViewModel);

                await _unitOfWork.EmployeeRepository.Add(mappedEmployee);
                return RedirectToAction("Index");
            }
            return View(employeeViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id is null)
                return NotFound();

            //var employee = await _unitOfWork.EmployeeRepository.Get(id);

            //var mappedEmployee = _mapper.Map<EmployeeViewModel>(employee);

            //var departmentName = await _unitOfWork.EmployeeRepository.GetDepartmentByEmployeeId(id);

            var employee = await _unitOfWork.EmployeeRepository.Get(id);

            ViewBag.Departments = await _unitOfWork.DepartmentRepository.GetAll();

            var mappedEmployee = _mapper.Map<EmployeeViewModel>(employee);


            if (employee is null)
                return NotFound();

            return View(mappedEmployee);

        }

        public async Task<IActionResult> Update(int? id)
        {

            if (id is null)
                return NotFound();

            var employee = await _unitOfWork.EmployeeRepository.Get(id);

            ViewBag.Departments = await _unitOfWork.DepartmentRepository.GetAll();

            var mappedEmployee = _mapper.Map<EmployeeViewModel>(employee);


            if (employee is null)
                return NotFound();

            return View(mappedEmployee);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int? id, EmployeeViewModel employeeViewModel)
        {

            if (id != employeeViewModel.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    employeeViewModel.ImgUrl = DocumentSettings.UploadFile(employeeViewModel.Image, "Imgs");
                    var mappedEmployee = _mapper.Map<Employee>(employeeViewModel);
                    await _unitOfWork.EmployeeRepository.Update(mappedEmployee);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    return View(employeeViewModel);
                }
            }

            return View(employeeViewModel);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null)
                return NotFound();

            var employee = await _unitOfWork.EmployeeRepository.Get(id);

            if (employee is null)
                return NotFound();

            DocumentSettings.DelteFile("Imgs", employee.ImgUrl);
            await _unitOfWork.EmployeeRepository.Delete(employee);
            return RedirectToAction("Index");
        }
    }
}
