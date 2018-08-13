using System.Diagnostics;
using FluentStoredProcedureExtensions.Core.Abstract;
using Microsoft.AspNetCore.Mvc;
using FluentStoredProcedureExtensions.WebUI.Models;

namespace FluentStoredProcedureExtensions.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;

        public HomeController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
