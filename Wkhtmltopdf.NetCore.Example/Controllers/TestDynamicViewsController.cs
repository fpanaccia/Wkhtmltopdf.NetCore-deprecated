using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Wkhtmltopdf.Models;
using Wkhtmltopdf.NetCore;

namespace Wkhtmltopdf.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestDynamicViewsController : Controller
    {
        private readonly IGeneratePdf _generatePdf;

        private readonly string htmlView = @"@model Wkhtmltopdf.Models.TestData
                        <!DOCTYPE html>
                        <html>
                        <head>
                        </head>
                        <body>
                            <header>
                                <h1>@Model.Text</h1>
                            </header>
                            <div>
                                <h2>@Model.Number</h2>
                            </div>
                        </body>
                        </html>";

        public TestDynamicViewsController(IGeneratePdf generatePdf)
        {
            _generatePdf = generatePdf;
        }

        /// <summary>
        /// String view pdf generation as ActionResult
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetByRazorText")]
        public async Task<IActionResult> GetByRazorText()
        {
            var data = new TestData
            {
                Text = "This is not a test",
                Number = 12345678
            };

            return await _generatePdf.GetPdfViewInHtml(htmlView, data);
        }

        /// <summary>
        /// string view pdf generation as ByteArray
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetByteArray")]
        public async Task<IActionResult> GetByteArray()
        {
            var data = new TestData
            {
                Text = "This is not a test",
                Number = 12345678
            };

            var pdf = await _generatePdf.GetByteArrayViewInHtml(htmlView, data);
            var pdfStream = new System.IO.MemoryStream();
            pdfStream.Write(pdf, 0, pdf.Length);
            pdfStream.Position = 0;
            return new FileStreamResult(pdfStream, "application/pdf");
        }

        /// <summary>
        /// Cached view pdf generation as ActionResult
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetFromEngine")]
        public async Task<IActionResult> GetFromEngine()
        {
            var data = new TestData
            {
                Text = "This is a test",
                Number = 123456
            };

            if (!_generatePdf.ExistsView("notAView"))
            {
                var html = await System.IO.File.ReadAllTextAsync("Views/Test.cshtml");
                _generatePdf.AddView("notAView", html);
            }

            return await _generatePdf.GetPdf("notAView", data);
        }

        /// <summary>
        /// Cached view and update view with string view pdf generation as ActionResult
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("UpdateAndGetFromEngine")]
        public async Task<IActionResult> UpdateAndGetFromEngine()
        {
            var data = new TestData
            {
                Text = "This is a test",
                Number = 123456
            };

            if (!_generatePdf.ExistsView("notAView"))
            {
                var html = await System.IO.File.ReadAllTextAsync("Views/Test.cshtml");
                _generatePdf.AddView("notAView", html);
            }
            else
            {
                var html = @"@model Wkhtmltopdf.Models.TestData
                        <!DOCTYPE html>
                        <html>
                        <head>
                        </head>
                        <body>
                            <header>
                                <h1>@Model.Text</h1>
                            </header>
                            <div>
                                <h2>Repeat @Model.Text</h2>
                            </div>
                            <div>
                                <h5>@Model.Number</h2>
                            </div>
                        </body>
                        </html>";

                _generatePdf.UpdateView("notAView", html);
            }

            return await _generatePdf.GetPdf("notAView", data);
        }
    }
}