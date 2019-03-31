using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;

namespace Wkhtmltopdf.NetCore
{
    public class UpdateableFileProvider : IFileProvider
    {
        public CancellationTokenSource _pagesTokenSource = new CancellationTokenSource();

        private readonly Dictionary<string, TestFileInfo> _content = new Dictionary<string, TestFileInfo>()
        {
            {
                "/Views/FakeView.cshtml",
                new TestFileInfo(string.Empty)
            }
        };

        public IDirectoryContents GetDirectoryContents(string subpath)
        {
            return new NotFoundDirectoryContents();
        }

        public void UpdateContent(string content)
        {
            var old = _content["/Views/FakeView.cshtml"];
            old.TokenSource.Cancel();
            _content["/Views/FakeView.cshtml"] = new TestFileInfo(content);
        }

        public void CancelRazorPages()
        {
            var oldToken = _pagesTokenSource;
            _pagesTokenSource = new CancellationTokenSource();
            oldToken.Cancel();
        }

        public IFileInfo GetFileInfo(string subpath)
        {
            if (!_content.TryGetValue("/Views/FakeView.cshtml", out var fileInfo))
            {
                fileInfo = new TestFileInfo(null);
            }

            return fileInfo;
        }

        public IChangeToken Watch(string filter)
        {
            if (filter == "/Pages/**/*.cshtml")
            {
                return new CancellationChangeToken(_pagesTokenSource.Token);
            }

            if (_content.TryGetValue(filter, out var fileInfo))
            {
                return fileInfo.ChangeToken;
            }

            return NullChangeToken.Singleton;
        }

        private class TestFileInfo : IFileInfo
        {
            private readonly string _content;

            public TestFileInfo(string content)
            {
                _content = content;
                ChangeToken = new CancellationChangeToken(TokenSource.Token);
                Exists = _content != null;
            }

            public bool Exists { get; }
            public bool IsDirectory => false;
            public DateTimeOffset LastModified => DateTimeOffset.MinValue;
            public long Length => -1;
            public string Name { get; set; }
            public string PhysicalPath => null;
            public CancellationTokenSource TokenSource { get; } = new CancellationTokenSource();
            public CancellationChangeToken ChangeToken { get; }

            public Stream CreateReadStream()
            {
                return new MemoryStream(Encoding.UTF8.GetBytes(_content));
            }
        }
    }
}
