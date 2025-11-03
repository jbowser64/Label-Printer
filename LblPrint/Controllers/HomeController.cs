using DataFirstTest.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Diagnostics;
using System.Security.Policy;

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

            //string mystring = ""; test
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
                parts = parts.Where(s => s.Material.Contains(printNum) && s.Sloc.Equals("1000"));

                foreach (var item in parts)
                {
                    string description = item.Description ?? "No Description";
                    string desc1 = description.Length > 25 ? description.Substring(0, 25) : description;
                    string desc2 = description.Length > 25 ? description.Substring(25) : "";

                    string URL = $"https://api.labelary.com/v1/printers/8dpmm/labels/2x1/0/" +
                       $"%5EXA%5EPW406%5EFT40,52%5EA0N,42,42%5EFH/%5EFD{item.Material}%5EFS%5EFT40,78%5EA0N,25,25%5EFH/%5E" +
                       $"FD{desc1}%5EFS%5EFT40,106%5EA0N,25,25%5EFH/%5E" +
                       $"FD{desc2}%5EFS%5EFT40,140%5EA0N,37,37%5EFH/%5E" +
                       $"FD{item.Bin}%5EFS%5EFT275,140%5EA0N,37,37%5EFH/%5EFD%5EFS%5EFT40,180%5EA0N,37,37%5EFH/%5E" +
                       $"FDFIFO: {DateTime.Now.ToString("MMMM yyyy")}%5EFS%5EPQ1,0,1,Y%5EXZ";

                    /*string URL = $"http://api.labelary.com/v1/printers/8dpmm/labels/2x1/0/%5EXA%5EPW406%5EFT40,52%5EA0N,42,42%5EFH/%5EFD{item.Material}" +
                        $"%5EFS%5EFT40,78%5EA0N,25,25%5EFH/%5EFD{item.Description}" +
                        $"%5EFS%5EFT40,106%5EA0N,25,25%5EFH/%5EFD%5EFS%5EFT40,140%5EA0N,37,37%5EFH/%5EFD{item.Bin}" +
                        $"%5EFS%5EFT275,140%5EA0N,37,37%5EFH/%5EFD%5EFS%5EFT40,180%5EA0N,37,37%5EFH/%5EFD{DateTime.Now.ToString("MMMM yyyy")}%20%5EFS%5EPQ1,0,1,Y%5EXZ";
                   */
                    ViewBag.url = URL;
                }

                return View(parts.ToList());
            }

            else

                return View();

        }

        public IActionResult PrintLabels(string printNum, int QTY, string FIFO, string Bin)
        {
            var parts = from p
                       in _context.PartsAndLocations
                       select p;
            if (!String.IsNullOrEmpty(printNum))
            {
                List<string> partChanger = new List<string>();

                parts = parts.Where(s => s.Material.Contains(printNum) && s.Sloc.Equals("1000"));
            
                foreach (var item in parts)
                {
                    partChanger.Add(item.ToString()); 
                }
                return View(parts.ToList());
            }
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
