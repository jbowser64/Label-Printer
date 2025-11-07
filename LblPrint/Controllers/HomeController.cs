using DataFirstTest.Models;
using DataFirstTest.PrintManager;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Text;

namespace DataFirstTest.Controllers
{
    public class HomeController : Controller
    {
        private readonly PartsAndLocationsContext _context;
        private readonly PrintOperations _printOperations;
        private const string DEFAULT_STORAGE_LOCATION = "1000";
        private const int MAX_DESCRIPTION_LENGTH = 25;

        public HomeController(PartsAndLocationsContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _printOperations = new PrintOperations(_context);
        }

        public async Task<IActionResult> PartsDB(string searchNum)
        {
            if (_context.PartsAndLocations == null)
            {
                return Problem("Database context is not properly configured.");
            }

            if (string.IsNullOrWhiteSpace(searchNum))
            {
                return View(new List<PartsAndLocation>());
            }

            var parts = await _context.PartsAndLocations
                .Where(p => p.Material != null && p.Material.Contains(searchNum))
                .ToListAsync();

            return View(parts);
        }

        public async Task<IActionResult> Index(string printNum)
        {
            if (_context.PartsAndLocations == null)
            {
                return Problem("Database context is not properly configured.");
            }

            if (string.IsNullOrWhiteSpace(printNum))
            {
                return View(new List<PartsAndLocation>());
            }

            var parts = await _context.PartsAndLocations
                .Where(p => p.Material != null && 
                           p.Material.Contains(printNum) && 
                           p.Sloc == DEFAULT_STORAGE_LOCATION)
                .ToListAsync();

            if (parts.Any())
            {
                // Generate label URL for the first matching part
                var firstPart = parts.First();
                ViewBag.url = GenerateLabelaryUrl(firstPart);
            }

            return View(parts);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DownloadLabel(string printNum, int qty = 1, string? fifo = null, string? bin = null)
        {
            if (string.IsNullOrWhiteSpace(printNum))
            {
                return BadRequest("Part number is required.");
            }

            try
            {
                var imageBytes = await _printOperations.DownloadLabelImageAsync(printNum, qty, fifo, bin);
                var fileName = $"Label_{printNum}_{DateTime.Now:yyyyMMddHHmmss}.png";
                
                return File(imageBytes, "image/png", fileName);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return Problem($"An error occurred while generating the label: {ex.Message}");
            }
        }

        public async Task<IActionResult> PrintLabels(string printNum, int qty = 1, string? fifo = null, string? bin = null)
        {
            if (_context.PartsAndLocations == null)
            {
                return Problem("Database context is not properly configured.");
            }

            if (string.IsNullOrWhiteSpace(printNum))
            {
                return View(new List<PartsAndLocation>());
            }

            var parts = await _context.PartsAndLocations
                .Where(p => p.Material != null && 
                           p.Material.Contains(printNum) && 
                           p.Sloc == DEFAULT_STORAGE_LOCATION)
                .ToListAsync();

            return View(parts);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel 
            { 
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier 
            });
        }

        /// <summary>
        /// Generates a Labelary API URL for generating a label image.
        /// </summary>
        /// <param name="part">The part information to include on the label</param>
        /// <returns>URL-encoded Labelary API URL</returns>
        private string GenerateLabelaryUrl(PartsAndLocation part)
        {
            if (part == null)
            {
                throw new ArgumentNullException(nameof(part));
            }

            var description = part.Description ?? "No Description";
            var desc1 = description.Length > MAX_DESCRIPTION_LENGTH 
                ? description.Substring(0, MAX_DESCRIPTION_LENGTH) 
                : description;
            var desc2 = description.Length > MAX_DESCRIPTION_LENGTH 
                ? description.Substring(MAX_DESCRIPTION_LENGTH) 
                : string.Empty;
            var material = part.Material ?? string.Empty;
            var bin = part.Bin ?? string.Empty;
            var fifoDate = DateTime.Now.ToString("MMMM yyyy");

            // Build ZPL command string
            var zplCommand = new StringBuilder();
            zplCommand.Append("^XA"); // Start label
            zplCommand.Append("^PW406"); // Print width
            zplCommand.Append("^FT40,52^A0N,42,42^FH\\^FD"); // Material number position and font
            zplCommand.Append(material);
            zplCommand.Append("^FS");
            zplCommand.Append("^FT40,78^A0N,25,25^FH\\^FD"); // Description line 1
            zplCommand.Append(desc1);
            zplCommand.Append("^FS");
            zplCommand.Append("^FT40,106^A0N,25,25^FH\\^FD"); // Description line 2
            zplCommand.Append(desc2);
            zplCommand.Append("^FS");
            zplCommand.Append("^FT40,140^A0N,37,37^FH\\^FD"); // Bin location
            zplCommand.Append(bin);
            zplCommand.Append("^FS");
            zplCommand.Append("^FT275,140^A0N,37,37^FH\\^FD^FS"); // Empty field
            zplCommand.Append("^FT40,180^A0N,37,37^FH\\^FD"); // FIFO date
            zplCommand.Append($"FIFO: {fifoDate}");
            zplCommand.Append("^FS");
            zplCommand.Append("^PQ1,0,1,Y"); // Print quantity
            zplCommand.Append("^XZ"); // End label

            // URL encode the ZPL command
            var encodedZpl = Uri.EscapeDataString(zplCommand.ToString());
            return $"https://api.labelary.com/v1/printers/8dpmm/labels/2x1/0/{encodedZpl}";
        }
    }
}
