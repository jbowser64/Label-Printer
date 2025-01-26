using DataFirstTest.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.Identity.Client;
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
        HttpClient client = new HttpClient(); 
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
                parts = parts.Where(s => s.Material.Contains(printNum));

                foreach (var item in parts)
                {
                    string URL = $"http://api.labelary.com/v1/printers/8dpmm/labels/2x1/0/%5EXA%5EPW406%5EFT40,52%5EA0N,42,42%5EFH/%5EFD{item.Material}%5EFS%5EFT40,78%5EA0N,25,25%5EFH/%5EFD{item.Description}%5EFS%5EFT40,106%5EA0N,25,25%5EFH/%5EFD32%5EFS%5EFT40,140%5EA0N,37,37%5EFH/%5EFD{item.Bin}%5EFS%5EFT275,140%5EA0N,37,37%5EFH/%5EFD%5EFS%5EFT40,180%5EA0N,37,37%5EFH/%5EFD{DateTime.Now.ToString("MMMM yyyy")}%202025%5EFS%5EPQ1,0,1,Y%5EXZ";
                }


                return View(parts.ToList());
            }

            //var parts = (from p in _context.PartsAndLocations where p.Material == "332042-0050" select p).ToList();
            else
                return View();


        }
        public async Task GetLabelFromAPI(string printNum)
        {
            var parts = from p
                       in _context.PartsAndLocations
                        select p;
            parts = parts.Where(s => s.Material.Contains(printNum));

            string apiURL = ""; 
            foreach (var item in parts)
            { 
                
            string s = $"http://api.labelary.com/v1/printers/8dpmm/labels/2x1/0/%5EXA%5EPW406%5EFT40,52%5EA0N,42,42%5EFH/%5EFD{item.Material}%5EFS%5EFT40,78%5EA0N,25,25%5EFH/%5EFD{item.Description}%5EFS%5EFT40,106%5EA0N,25,25%5EFH/%5EFD32%5EFS%5EFT40,140%5EA0N,37,37%5EFH/%5EFD{item.Bin}%5EFS%5EFT275,140%5EA0N,37,37%5EFH/%5EFD%5EFS%5EFT40,180%5EA0N,37,37%5EFH/%5EFD{DateTime.Now.ToString("MMMM yyyy")}%202025%5EFS%5EPQ1,0,1,Y%5EXZ";
                apiURL = s; 
            }


            var response = await client.GetAsync(apiURL);
            
            var responseStream = await response.Content.ReadAsStreamAsync();

            var image = File.Create("image.png");
            responseStream.CopyTo(image);
            responseStream.Close();
            return (image);
        }
        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
   
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
