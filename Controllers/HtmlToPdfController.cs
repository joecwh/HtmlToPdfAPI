using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SelectPdf;

namespace HtmlToPdfConversion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HtmlToPdfController : ControllerBase
    {
        [HttpPost("ConvertHtmlFileToPdf")]
        public IActionResult ConvertHtmlFileToPdf(IFormFile htmlFile)
        {
            try
            {
                // Read HTML content from the uploaded file
                string htmlContent;
                using (var reader = new StreamReader(htmlFile.OpenReadStream()))
                {
                    htmlContent = reader.ReadToEnd();
                }

                // Convert HTML to PDF
                var converter = new HtmlToPdf();
                var doc = converter.ConvertHtmlString(htmlContent);

                // Save the PDF to the desktop folder
                var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                var pdfPath = Path.Combine(desktopPath, "outputFromHtml.pdf");
                doc.Save(pdfPath);
                doc.Close();

                return Ok("PDF generated and saved to the desktop folder.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("ConvertHtmlStringToPdf")]
        public IActionResult ConvertHtmlStringToPdf([FromBody] string htmlContent)
        {
            try
            {
                // Create a new SelectPdf converter
                HtmlToPdf converter = new HtmlToPdf();

                // Convert the HTML content to PDF
                PdfDocument doc = converter.ConvertHtmlString(htmlContent);

                // Save the PDF to the desktop folder
                string desktopPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);
                string pdfFilePath = System.IO.Path.Combine(desktopPath, "outputFromString.pdf");
                doc.Save(pdfFilePath);
                doc.Close();

                // Return a response with the PDF file path
                return Ok("PDF generated and saved to the desktop folder.");
            }
            catch (Exception ex)
            {
                // Handle any errors that occur during conversion
                return BadRequest(new { Error = ex.Message });
            }
        }
    }
}
