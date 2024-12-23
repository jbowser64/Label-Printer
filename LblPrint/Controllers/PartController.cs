using LblPrint.Data;
using LblPrint.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LblPrint.Controllers
{
    public class PartController : Controller
    {
        private PartsDatabaseContext db = new PartsDatabaseContext();

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
