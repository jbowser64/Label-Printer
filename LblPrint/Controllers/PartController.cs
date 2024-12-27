using LblPrint.Data;
using LblPrint.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LblPrint.Controllers
{
    public class PartController : Controller
    {
        private readonly PartsDatabaseContext _context;

        public PartController(PartsDatabaseContext context)
        {
            _context = context;
        }

        private PartsDatabaseContext db = new PartsDatabaseContext();

        [HttpPost]
        public IActionResult PrintLabel(string partNum)
        {
            List<PartModel> parts = new List<PartModel>();

            var result =
            from e in db.Parts
            where e.Material == partNum
            select new PartModel
            {
                Material = e.Material,
                Material_description = e.Material_description,
                Bin = e.Bin
            };
            parts = result.ToList();
            return View(parts);

            /* 
            string partNum = Request.Form["partNum"].ToString();
            var parts =
            from e in db.Parts
            where e.Material == partNum
            select e;

            parts.ToArray(); 
            

            return View(parts);
            */
        }
    }
}
