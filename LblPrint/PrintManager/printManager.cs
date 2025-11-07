using DataFirstTest.Models;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace DataFirstTest.PrintManager
{
    public class PrintOperations
    {
        private readonly PartsAndLocationsContext _context;
        private const string DEFAULT_STORAGE_LOCATION = "1000";
        private const int MAX_DESCRIPTION_LENGTH = 25;

        public PrintOperations(PartsAndLocationsContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Generates a label URL for the Labelary API.
        /// </summary>
        /// <param name="partNum">Part number to search for</param>
        /// <param name="quantity">Number of labels to generate (affects PQ command)</param>
        /// <param name="customFifo">Optional custom FIFO date, uses current month/year if null</param>
        /// <param name="customBin">Optional custom bin location</param>
        /// <returns>Labelary API URL for the label image</returns>
        public string GenerateLabelUrl(string partNum, int quantity = 1, string? customFifo = null, string? customBin = null)
        {
            if (string.IsNullOrWhiteSpace(partNum))
            {
                throw new ArgumentException("Part number cannot be null or empty.", nameof(partNum));
            }

            var part = _context.PartsAndLocations
                .FirstOrDefault(p => p.Material != null && 
                                    p.Material.Contains(partNum) && 
                                    p.Sloc == DEFAULT_STORAGE_LOCATION);

            if (part == null)
            {
                throw new InvalidOperationException($"Part '{partNum}' not found in storage location {DEFAULT_STORAGE_LOCATION}.");
            }

            var description = part.Description ?? "No Description";
            var desc1 = description.Length > MAX_DESCRIPTION_LENGTH 
                ? description.Substring(0, MAX_DESCRIPTION_LENGTH) 
                : description;
            var desc2 = description.Length > MAX_DESCRIPTION_LENGTH 
                ? description.Substring(MAX_DESCRIPTION_LENGTH) 
                : string.Empty;
            var material = part.Material ?? string.Empty;
            var bin = customBin ?? part.Bin ?? string.Empty;
            var fifoDate = customFifo ?? DateTime.Now.ToString("MMMM yyyy");

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
            zplCommand.Append($"^PQ{quantity},0,1,Y"); // Print quantity
            zplCommand.Append("^XZ"); // End label

            // URL encode the ZPL command
            var encodedZpl = Uri.EscapeDataString(zplCommand.ToString());
            return $"https://api.labelary.com/v1/printers/8dpmm/labels/2x1/0/{encodedZpl}";
        }

        /// <summary>
        /// Downloads a label image from the Labelary API as a byte array.
        /// </summary>
        /// <param name="partNum">Part number to generate label for</param>
        /// <param name="quantity">Number of labels (default: 1)</param>
        /// <param name="customFifo">Optional custom FIFO date</param>
        /// <param name="customBin">Optional custom bin location</param>
        /// <returns>Byte array containing the PNG image data</returns>
        public async Task<byte[]> DownloadLabelImageAsync(string partNum, int quantity = 1, string? customFifo = null, string? customBin = null)
        {
            var labelUrl = GenerateLabelUrl(partNum, quantity, customFifo, customBin);

            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Accept", "image/png");
            client.DefaultRequestHeaders.Add("User-Agent", "TA Label Printer App");

            try
            {
                var imageBytes = await client.GetByteArrayAsync(labelUrl);
                return imageBytes;
            }
            catch (HttpRequestException ex)
            {
                throw new InvalidOperationException(
                    $"Error downloading label image from Labelary API. The server may be unavailable or the URL is invalid. {ex.Message}", 
                    ex);
            }
        }
    }
}
