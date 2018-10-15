using Microsoft.AspNetCore.Mvc;
using RazorLight;
using System.Dynamic;
using System.IO;
using System.Threading.Tasks;

namespace Wkhtmltopdf.NetCore
{
    public class ViewAsPdf : HtmlAsPdf
    {
        RazorLightEngine _engine;
        public ViewAsPdf(string viewsPath = null)
        {
            _engine = new RazorLightEngineBuilder()
                            .UseFilesystemProject(viewsPath)
                            .UseMemoryCachingProvider()
                            .Build();
        }

        public ViewAsPdf()
        {
            _engine = new RazorLightEngineBuilder()
                            .UseFilesystemProject(Directory.GetCurrentDirectory())
                            .UseMemoryCachingProvider()
                            .Build();
        }

        public async Task<byte[]> GetByteArray<T>(string View, T model, ExpandoObject viewBag = null)
        {
            try
            {
                var html = await _engine.CompileRenderAsync(View, model, viewBag);
                return base.GetPDF(html);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IActionResult> GetPdf<T>(string View, T model, ExpandoObject viewBag = null)
        {
            var html = await _engine.CompileRenderAsync(View, model, viewBag);
            var byteArray = base.GetPDF(html);
            MemoryStream pdfStream = new MemoryStream();
            pdfStream.Write(byteArray, 0, byteArray.Length);
            pdfStream.Position = 0;
            return new FileStreamResult(pdfStream, "application/pdf");
        }
    }
}