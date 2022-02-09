using System;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Wkhtmltopdf.NetCore.Interfaces;

namespace Wkhtmltopdf.NetCore
{
    public interface IGeneratePdf
    {
        Task<IActionResult> GetPdfViewInHtml<T>(string ViewInHtml, T model);
        Task<byte[]> GetByteArrayViewInHtml<T>(string ViewInHtml, T model);
        Task<IActionResult> GetPdf<T>(string View, T model);
        Task<IActionResult> GetPdf<T>(Uri url);
        Task<byte[]> GetByteArray<T>(string View, T model);
        Task<IActionResult> GetPdfViewInHtml(string ViewInHtml);
        Task<byte[]> GetByteArrayViewInHtml(string ViewInHtml);
        Task<IActionResult> GetPdf(string View);
        Task<IActionResult> GetPdf(Uri url);
        Task<byte[]> GetByteArray(string View);
        void SetConvertOptions(IConvertOptions options);
        Task<byte[]> GetPDF(string html);
        Task<byte[]> GetPDF(Uri url);
        void UpdateView(string path, string viewHTML);
        bool ExistsView(string path);
        void AddView(string path, string viewHTML);
    }
}