using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;
using System;
using System.IO;
using System.Text;
using System.Threading;

namespace Wkhtmltopdf.NetCore
{
    public class ViewFileInfo : IFileInfo
    {
        public readonly string Content;

        public ViewFileInfo(string content)
        {
            Content = content;
            ChangeToken = new CancellationChangeToken(TokenSource.Token);
            Exists = Content != null;
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
            return new MemoryStream(Encoding.UTF8.GetBytes(Content));
        }
    }
}
