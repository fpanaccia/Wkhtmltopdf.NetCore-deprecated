using System.Threading.Tasks;

namespace Wkhtmltopdf.NetCore
{
    public interface IRazorViewToStringRenderer
    {
        Task<string> RenderViewToStringAsync<TModel>(string viewName, TModel model);
        Task<string> RenderHtmlToStringAsync<TModel>(string html, TModel model);
        void UpdateView(string path, string viewHTML);
        bool ExistsView(string path);
        void AddView(string path, string viewHTML);
    }
}
