using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using System;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Wkhtmltopdf.NetCore
{
    public class GeneratePdf : AsPdfResultBase, IGeneratePdf
    {
        readonly IRazorViewToStringRenderer _engine;
        public GeneratePdf(IRazorViewToStringRenderer engine)
        {
            _engine = engine;
        }

        public byte[] GetPDF(string html)
        {
            return WkhtmlDriver.Convert(WkhtmltopdfConfiguration.RotativaPath, this.GetConvertOptions(), html);
        }

        public async Task<byte[]> GetByteArray<T>(string View, T model)
        {
            try
            {
                
                var html = await _engine.RenderViewToStringAsync(View, model);
                return GetPDF(html);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IActionResult> GetPdf<T>(string View, T model)
        {
            var html = await _engine.RenderViewToStringAsync(View, model);
            var byteArray = GetPDF(html);
            MemoryStream pdfStream = new MemoryStream();
            pdfStream.Write(byteArray, 0, byteArray.Length);
            pdfStream.Position = 0;
            return new FileStreamResult(pdfStream, "application/pdf");
        }

        public async Task<IActionResult> GetPdfViewInHtml<T>(string ViewInHtml, T model)
        {
            var html = await _engine.RenderHtmlToStringAsync(ViewInHtml, model);
            var byteArray = GetPDF(html);
            MemoryStream pdfStream = new MemoryStream();
            pdfStream.Write(byteArray, 0, byteArray.Length);
            pdfStream.Position = 0;
            return new FileStreamResult(pdfStream, "application/pdf");
        }

        public async Task<byte[]> GetByteArrayViewInHtml<T>(string ViewInHtml, T model)
        {
            try
            {
                var view = await _engine.RenderHtmlToStringAsync(ViewInHtml, model);
                return GetPDF(view);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    public interface IGeneratePdf
    {
        Task<IActionResult> GetPdfViewInHtml<T>(string ViewInHtml, T model);
        Task<byte[]> GetByteArrayViewInHtml<T>(string ViewInHtml, T model);
        Task<IActionResult> GetPdf<T>(string View, T model);
        Task<byte[]> GetByteArray<T>(string View, T model);
        byte[] GetPDF(string html);
    }
}