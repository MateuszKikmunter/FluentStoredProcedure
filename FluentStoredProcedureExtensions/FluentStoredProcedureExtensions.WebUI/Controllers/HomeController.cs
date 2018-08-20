using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentStoredProcedureExtensions.Core.Abstract;
using FluentStoredProcedureExtensions.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using FluentStoredProcedureExtensions.WebUI.Models;
using FluentStoredProcedureExtensions.WebUI.ViewModels;

namespace FluentStoredProcedureExtensions.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;

        public HomeController(IMapper mapper, IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
            _mapper = mapper;
        }


        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetEmployees()
        {
            try
            {
                var emps = await _employeeRepository.GetAllAsync();
                var result = _mapper.Map<List<EmployeeViewModel>>(emps);
                return Json(new { data = result });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpDelete("{id}")]
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
        public async Task<IActionResult> Create([FromBody] EmployeeForManipulationViewModel employee)
        {
            if (employee == null)
            {
                return BadRequest();
            }

            var employeeToAdd = _mapper.Map<Employee>(employee);
            var creationSuccessful = await _employeeRepository.CreateAsync(employeeToAdd);
            if (creationSuccessful)
            {
                return Ok();
            }

            return BadRequest();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] EmployeeForManipulationViewModel employee)
        {
            if (employee == null)
            {
                return BadRequest();
            }

            var employeeToUpdate = await _employeeRepository.GetSingleAsync(id);
            if (employeeToUpdate == null)
            {
                return NotFound();
            }

            await _employeeRepository.UpdateAsync(id, employeeToUpdate);
            return Ok();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
