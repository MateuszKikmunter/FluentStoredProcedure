using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using FluentStoredProcedureExtensions.Core.Abstract;
using FluentStoredProcedureExtensions.Core.Entities;
using FluentStoredProcedureExtensions.WebUI.Helpers;
using FluentStoredProcedureExtensions.WebUI.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace FluentStoredProcedureExtensions.WebUI.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;

        public EmployeeController(IEmployeeRepository employeeRepository, IMapper mapper)
        {
            _employeeRepository = employeeRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetEmployees()
        {
            var employees = await _employeeRepository.GetAllAsync();
            var result = _mapper.Map<List<EmployeeViewModel>>(employees);
            return Json(new { data = result });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] int id)
        {
            var deletionSuccessful = await _employeeRepository.DeleteAsync(id);
            if (deletionSuccessful)
            {
                return Ok();
            }

            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromBody] CreateEmployeeViewModel employee)
        {
            if (employee == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return new ValidationFailedResult(ModelState);
            }

            var employeeToAdd = _mapper.Map<Employee>(employee);
            var creationSuccessful = await _employeeRepository.CreateAsync(employeeToAdd);
            if (creationSuccessful)
            {
                return Ok();
            }

            return BadRequest();
        }

        [HttpPut]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update([FromBody] EditEmployeeViewModel employee)
        {
            if (employee == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return new ValidationFailedResult(ModelState);
            }

            var employeeToUpdate = await _employeeRepository.GetSingleAsync(employee.Id);
            if (employeeToUpdate == null)
            {
                return NotFound();
            }

            await _employeeRepository.UpdateAsync(employeeToUpdate.Id, _mapper.Map<Employee>(employee));
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Get(int id)
        {
            var employee = await _employeeRepository.GetSingleAsync(id);
            if (employee == null)
            {
                return PartialView("_ManipulateEmployeePartial", new CreateEmployeeViewModel());
            }

            ViewBag.EployeeId = employee.Id;
            return PartialView("_ManipulateEmployeePartial", _mapper.Map<EditEmployeeViewModel>(employee));
        }
    }
}
