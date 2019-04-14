using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Wkhtmltopdf.NetCore
{
    public interface IGeneratePdf
    {
        Task<IActionResult> GetPdfViewInHtml<T>(string ViewInHtml, T model);
        Task<byte[]> GetByteArrayViewInHtml<T>(string ViewInHtml, T model);
        Task<IActionResult> GetPdf<T>(string View, T model);
        Task<byte[]> GetByteArray<T>(string View, T model);
        byte[] GetPDF(string html);
        void UpdateView(string path, string viewHTML);
        bool ExistsView(string path);
        void AddView(string path, string viewHTML);
    }
}