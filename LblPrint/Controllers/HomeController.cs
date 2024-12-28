using System.Diagnostics;
using System.Reflection.Metadata;
using Azure.Core;
using LblPrint.Data;
using LblPrint.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Storage;

namespace LblPrint.Controllers
{
    public class HomeController : Controller
    {
       // private readonly ILogger<HomeController> _logger;

        private readonly PartsDatabaseContext _context;

        public HomeController(PartsDatabaseContext context)
        {
            _context = context;
        }
        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}
        public IActionResult Index()
        {
            return View();
        }


        public IActionResult PartsDB()
        {
            var parts = _context.ViewParts.ToList();
            return View(parts);
          
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
