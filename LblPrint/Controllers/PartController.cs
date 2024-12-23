using LblPrint.Data;
using LblPrint.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LblPrint.Controllers
{
    public class PartController : Controller
    {
        private PartsDatabaseContext db = new PartsDatabaseContext();

        
        public IActionResult Index()
        {
            string partNum = Request.Form["partNum"].ToString();
            var parts =
            from e in db.Parts
            where e.Material == partNum
            select e;

            parts.ToArray(); 
            

            return View(parts);
        }
    }
}
