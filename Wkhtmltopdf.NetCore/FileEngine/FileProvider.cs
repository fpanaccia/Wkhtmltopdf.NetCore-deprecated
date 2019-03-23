using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Text;

namespace Wkhtmltopdf.NetCore.FileEngine
{
    public class FileProvider : IFileProvider, IFakeView
    {
        string _html;
        public FileProvider(string html)
        {
            _html = html;
        }

        public IDirectoryContents GetDirectoryContents(string subpath)
        {
            throw new NotImplementedException();
        }

        public IFileInfo GetFileInfo(string subpath)
        {
            return new FileInfo(_html);
        }

        public IChangeToken Watch(string filter)
        {
            throw new NotImplementedException();
        }
    }

    public interface IFakeView
    {

    }
}
