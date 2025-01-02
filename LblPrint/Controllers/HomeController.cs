using DataFirstTest.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.Entity;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Diagnostics;

namespace DataFirstTest.Controllers
{
    public class HomeController : Controller
    {
        private readonly PartsAndLocationsContext _context;

        public HomeController(PartsAndLocationsContext context)
        {
            _context = context;
        }

        public IActionResult PartsDB(string searchNum)
        {
            if (_context.PartsAndLocations == null)
            {

                return Problem($"{searchNum} not located in database");
            }

            var parts = from p
                        in _context.PartsAndLocations
                        select p;
            if (!String.IsNullOrEmpty(searchNum))
            {
                parts = parts.Where(s => s.Material.Contains(searchNum));
                return View(parts.ToList());
            }
            else
                return View();

           
        }

        public IActionResult Index(string printNum)
        {
            if (_context.PartsAndLocations == null)
            {
                return Problem($"{printNum} not located in database");
            }

            var parts = from p
                        in _context.PartsAndLocations
                        select p;
            if (!String.IsNullOrEmpty(printNum))
            {
                parts = parts.Where(s => s.Material.Contains(printNum));
                return View(parts.ToList());
            }

            //var parts = (from p in _context.PartsAndLocations where p.Material == "332042-0050" select p).ToList();
            else
                return View();
            
            
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
