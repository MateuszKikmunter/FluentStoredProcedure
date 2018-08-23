using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using FluentStoredProcedureExtensions.WebUI.Models;

namespace FluentStoredProcedureExtensions.WebUI.Controllers
{
    public class HomeController : Controller
    {
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
