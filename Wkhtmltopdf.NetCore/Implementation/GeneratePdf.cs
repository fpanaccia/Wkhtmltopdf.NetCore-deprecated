﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;
using Wkhtmltopdf.NetCore.Interfaces;

namespace Wkhtmltopdf.NetCore
{
    public class GeneratePdf : IGeneratePdf
    {
        protected IConvertOptions _convertOptions;
        readonly IRazorViewToStringRenderer _engine;

        public GeneratePdf(IRazorViewToStringRenderer engine)
        {
            _engine = engine;
            _convertOptions = new ConvertOptions();
        }

        public void SetConvertOptions(IConvertOptions convertOptions)
        {
            _convertOptions = convertOptions;
        }

        public Task<byte[]> GetPDF(string html)
        {
            return WkhtmlDriver.Convert(WkhtmltopdfConfiguration.RotativaPath, _convertOptions.GetConvertOptions(),
                html);
        }

        public Task<byte[]> GetPDF(Uri url)
        {
            return WkhtmlDriver.Convert(WkhtmltopdfConfiguration.RotativaPath, _convertOptions.GetConvertOptions(),
                url);
        }

        public async Task<byte[]> GetByteArray<T>(string View, T model)
        {
            try
            {
                var html = await _engine.RenderViewToStringAsync(View, model);
                return await GetPDF(html);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IActionResult> GetPdf<T>(string View, T model)
        {
            var html = await _engine.RenderViewToStringAsync(View, model);
            var byteArray = await GetPDF(html);
            MemoryStream pdfStream = new MemoryStream();
            await pdfStream.WriteAsync(byteArray, 0, byteArray.Length);
            pdfStream.Position = 0;
            return new FileStreamResult(pdfStream, "application/pdf");
        }

        public async Task<IActionResult> GetPdf<T>(Uri url)
        {
            var byteArray = await GetPDF(url);
            MemoryStream pdfStream = new MemoryStream();
            await pdfStream.WriteAsync(byteArray, 0, byteArray.Length);
            pdfStream.Position = 0;
            return new FileStreamResult(pdfStream, "application/pdf");
        }

        public async Task<IActionResult> GetPdfViewInHtml<T>(string ViewInHtml, T model)
        {
            var html = await _engine.RenderHtmlToStringAsync(ViewInHtml, model);
            var byteArray = await GetPDF(html);
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
                return await GetPDF(view);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddView(string path, string viewHTML) => _engine.AddView(path, viewHTML);

        public bool ExistsView(string path) => _engine.ExistsView(path);

        public void UpdateView(string path, string viewHTML) => _engine.UpdateView(path, viewHTML);

        public async Task<IActionResult> GetPdfViewInHtml(string ViewInHtml)
        {
            var html = await _engine.RenderHtmlToStringAsync(ViewInHtml);
            var byteArray = await GetPDF(html);
            MemoryStream pdfStream = new MemoryStream();
            pdfStream.Write(byteArray, 0, byteArray.Length);
            pdfStream.Position = 0;
            return new FileStreamResult(pdfStream, "application/pdf");
        }

        public async Task<byte[]> GetByteArrayViewInHtml(string ViewInHtml)
        {
            try
            {
                var view = await _engine.RenderHtmlToStringAsync(ViewInHtml);
                return await GetPDF(view);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IActionResult> GetPdf(string View)
        {
            var html = await _engine.RenderViewToStringAsync(View);
            var byteArray = await GetPDF(html);
            MemoryStream pdfStream = new MemoryStream();
            pdfStream.Write(byteArray, 0, byteArray.Length);
            pdfStream.Position = 0;
            return new FileStreamResult(pdfStream, "application/pdf");
        }

        public async Task<IActionResult> GetPdf(Uri url)
        {
            var byteArray = await GetPDF(url);
            MemoryStream pdfStream = new MemoryStream();
            await pdfStream.WriteAsync(byteArray, 0, byteArray.Length);
            pdfStream.Position = 0;
            return new FileStreamResult(pdfStream, "application/pdf");
        }

        public async Task<byte[]> GetByteArray(string View)
        {
            try
            {
                var html = await _engine.RenderViewToStringAsync(View);
                return await GetPDF(html);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}