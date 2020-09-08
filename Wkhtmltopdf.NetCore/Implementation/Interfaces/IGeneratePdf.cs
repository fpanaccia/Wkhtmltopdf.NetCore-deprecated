using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Wkhtmltopdf.NetCore.Interfaces;

namespace Wkhtmltopdf.NetCore
{
    public interface IGeneratePdf
    {
        Task<IActionResult> GetPdfViewInHtml<T>(string viewInHtml, T model);
        Task<byte[]> GetByteArrayViewInHtml<T>(string viewInHtml, T model);
        Task<IActionResult> GetPdf<T>(string view, T model);
        Task<byte[]> GetByteArray<T>(string view, T model);
        Task<IActionResult> GetPdfViewInHtml(string viewInHtml);
        Task<byte[]> GetByteArrayViewInHtml(string viewInHtml);
        Task<IActionResult> GetPdf(string view);
        Task<byte[]> GetByteArray(string view);
        void SetConvertOptions(IConvertOptions options);
        byte[] GetPDF(string html);
        void UpdateView(string path, string viewHtml);
        bool ExistsView(string path);
        void AddView(string path, string viewHtml);
    }
}