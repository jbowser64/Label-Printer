using LblPrint.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LblPrint.Controllers
{
    public class Parts : Controller
    {
        private partDBContext db = new partDBContext();

        public IActionResult Index(string partNum)
        {
            
            var parts =
            from e in db.GetData
            where e.Material == partNum.ToString()
            select e;

            parts.ToArray(); 
            

            return View(parts);
        }
    }
}
