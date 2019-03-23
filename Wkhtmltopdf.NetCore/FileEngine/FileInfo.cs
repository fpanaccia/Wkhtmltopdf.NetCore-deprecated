using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Wkhtmltopdf.NetCore.FileEngine
{
    public class FileInfo : IFileInfo
    {
        private byte[] _viewContent;

        public FileInfo(string html)
        {
            _viewContent = Encoding.UTF8.GetBytes(html);
        }

        public bool Exists => true;

        public long Length
        {
            get
            {
                using (var stream = new MemoryStream(_viewContent))
                {
                    return stream.Length;
                }
            }
        }

        public string PhysicalPath => null;

        public string Name => "fakeView";

        public DateTimeOffset LastModified => DateTime.Now.AddHours(-1);

        public bool IsDirectory => false;

        public Stream CreateReadStream()
        {
            return new MemoryStream(_viewContent);
        }
    }
}
