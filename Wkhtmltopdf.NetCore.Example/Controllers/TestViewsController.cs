using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Wkhtmltopdf.Models;
using Wkhtmltopdf.NetCore;

namespace Wkhtmltopdf.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestViewsController : Controller
    {
        readonly IGeneratePdf _generatePdf;
        public TestViewsController(IGeneratePdf generatePdf)
        {
            _generatePdf = generatePdf;
        }

        /// <summary>
        /// View pdf generation as ActionResult
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get()
        {
            var data = new TestData
            {
                Text = "This is a test",
                Number = 123456
            };

            return await _generatePdf.GetPdf("Views/Test.cshtml", data);
        }

        /// <summary>
        /// View pdf generation as ByteArray
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetByteArray")]
        public async Task<IActionResult> GetByteArray()
        {
            var data = new TestData
            {
                Text = "This is a test",
                Number = 123456
            };

            var pdf = await _generatePdf.GetByteArray("Views/Test.cshtml", data);
            var pdfStream = new System.IO.MemoryStream();
            pdfStream.Write(pdf, 0, pdf.Length);
            pdfStream.Position = 0;
            return new FileStreamResult(pdfStream, "application/pdf");
        }

        /// <summary>
        /// "Hardcode" html pdf generation as ByteArray
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetByHtml")]
        public IActionResult GetByHtml()
        {
            var html = @"<!DOCTYPE html>
                        <html>
                        <head>
                        </head>
                        <body>
                            <header>
                                <h1>This is a hardcoded test</h1>
                            </header>
                            <div>
                                <h2>456789</h2>
                            </div>
                        </body>";

            var pdf = _generatePdf.GetPDF(html);
            var pdfStream = new System.IO.MemoryStream();
            pdfStream.Write(pdf, 0, pdf.Length);
            pdfStream.Position = 0;
            return new FileStreamResult(pdfStream, "application/pdf");
        }

        /// <summary>
        /// "Hardcode" html pdf generation as ByteArray and save file
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("SaveByHtml")]
        public IActionResult SaveByHtml()
        {
            var html = @"<!DOCTYPE html>
                        <html>
                        <head>
                        </head>
                        <body>
                            <header>
                                <h1>This is a hardcoded test</h1>
                            </header>
                            <div>
                                <h2>456789</h2>
                            </div>
                        </body>";

            System.IO.File.WriteAllBytes("testHard.pdf", _generatePdf.GetPDF(html));
            return Ok();
        }

        /// <summary>
        /// View pdf generation as ByteArray and save file
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("SaveFile")]
        public async Task<IActionResult> SaveFile()
        {
            var data = new TestData
            {
                Text = "This is a test",
                Number = 123456
            };

            System.IO.File.WriteAllBytes("test.pdf", await _generatePdf.GetByteArray("Views/Test.cshtml", data));
            return Ok();
        }
    }
}
