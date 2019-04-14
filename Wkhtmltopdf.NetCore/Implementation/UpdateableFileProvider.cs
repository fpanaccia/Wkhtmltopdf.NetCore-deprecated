using System.Collections.Generic;
using System.Threading;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;

namespace Wkhtmltopdf.NetCore
{
    public class UpdateableFileProvider : IFileProvider
    {
        public CancellationTokenSource _pagesTokenSource = new CancellationTokenSource();

        public static Dictionary<string, ViewFileInfo> Views = new Dictionary<string, ViewFileInfo>()
        {
            {
                "/Views/FakeView.cshtml",
                new ViewFileInfo(string.Empty)
            }
        };

        public IDirectoryContents GetDirectoryContents(string subpath)
        {
            return new NotFoundDirectoryContents();
        }

        public static void UpdateContent(string content, string view = null)
        {
            var viewPath = string.IsNullOrWhiteSpace(view) ? "/Views/FakeView.cshtml" : view;
            var old = Views[viewPath];
            old.TokenSource.Cancel();
            Views[viewPath] = new ViewFileInfo(content);
        }

        public void CancelRazorPages()
        {
            var oldToken = _pagesTokenSource;
            _pagesTokenSource = new CancellationTokenSource();
            oldToken.Cancel();
        }

        public IFileInfo GetFileInfo(string subpath)
        {
            var viewPath = string.IsNullOrWhiteSpace(subpath) ? "/Views/FakeView.cshtml" : subpath;
            if (!Views.TryGetValue(viewPath, out var fileInfo))
            {
                fileInfo = new ViewFileInfo(null);
            }

            return fileInfo;
        }

        public IChangeToken Watch(string filter)
        {
            if (filter == "/Pages/**/*.cshtml")
            {
                return new CancellationChangeToken(_pagesTokenSource.Token);
            }

            if (Views.TryGetValue(filter, out var fileInfo))
            {
                return fileInfo.ChangeToken;
            }

            return NullChangeToken.Singleton;
        }
    }
}
