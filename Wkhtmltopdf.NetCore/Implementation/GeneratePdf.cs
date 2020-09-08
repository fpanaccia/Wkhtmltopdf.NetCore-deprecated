using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;
using Wkhtmltopdf.NetCore.Interfaces;

namespace Wkhtmltopdf.NetCore
{
    public class GeneratePdf : IGeneratePdf
    {
        private IConvertOptions _convertOptions;
        private readonly IRazorViewToStringRenderer _engine;

        public GeneratePdf(IRazorViewToStringRenderer engine)
        {
            _engine = engine;
            _convertOptions = new ConvertOptions();
        }

        public void SetConvertOptions(IConvertOptions convertOptions)
        {
            _convertOptions = convertOptions;
        }

        public byte[] GetPDF(string html)
        {
            return WkhtmlDriver.Convert(WkhtmltopdfConfiguration.RotativaPath, _convertOptions.GetConvertOptions(), html);
        }

        public async Task<byte[]> GetByteArray<T>(string view, T model)
        {
            var html = await _engine.RenderViewToStringAsync(view, model);
            return GetPDF(html);
        }

        public async Task<IActionResult> GetPdf<T>(string view, T model)
        {
            var html = await _engine.RenderViewToStringAsync(view, model);
            var byteArray = GetPDF(html);
            MemoryStream pdfStream = new MemoryStream();
            pdfStream.Write(byteArray, 0, byteArray.Length);
            pdfStream.Position = 0;
            return new FileStreamResult(pdfStream, "application/pdf");
        }

        public async Task<IActionResult> GetPdfViewInHtml<T>(string viewInHtml, T model)
        {
            var html = await _engine.RenderHtmlToStringAsync(viewInHtml, model);
            var byteArray = GetPDF(html);
            MemoryStream pdfStream = new MemoryStream();
            pdfStream.Write(byteArray, 0, byteArray.Length);
            pdfStream.Position = 0;
            return new FileStreamResult(pdfStream, "application/pdf");
        }

        public async Task<byte[]> GetByteArrayViewInHtml<T>(string viewInHtml, T model)
        {
            var view = await _engine.RenderHtmlToStringAsync(viewInHtml, model);
            return GetPDF(view);
        }

        public void AddView(string path, string viewHtml) => _engine.AddView(path, viewHtml);

        public bool ExistsView(string path) => _engine.ExistsView(path);

        public void UpdateView(string path, string viewHtml) => _engine.UpdateView(path, viewHtml);

        public async Task<IActionResult> GetPdfViewInHtml(string viewInHtml)
        {
            var html = await _engine.RenderHtmlToStringAsync(viewInHtml);
            var byteArray = GetPDF(html);
            MemoryStream pdfStream = new MemoryStream();
            pdfStream.Write(byteArray, 0, byteArray.Length);
            pdfStream.Position = 0;
            return new FileStreamResult(pdfStream, "application/pdf");
        }

        public async Task<byte[]> GetByteArrayViewInHtml(string viewInHtml)
        {
            var view = await _engine.RenderHtmlToStringAsync(viewInHtml);
            return GetPDF(view);
        }

        public async Task<IActionResult> GetPdf(string view)
        {
            var html = await _engine.RenderViewToStringAsync(view);
            var byteArray = GetPDF(html);
            MemoryStream pdfStream = new MemoryStream();
            pdfStream.Write(byteArray, 0, byteArray.Length);
            pdfStream.Position = 0;
            return new FileStreamResult(pdfStream, "application/pdf");
        }

        public async Task<byte[]> GetByteArray(string view)
        {
            var html = await _engine.RenderViewToStringAsync(view);
            return GetPDF(html);
        }
    }
}