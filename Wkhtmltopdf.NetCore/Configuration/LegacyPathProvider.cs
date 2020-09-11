using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Wkhtmltopdf.NetCore
{
    /// <summary>
    ///     Emulates legacy registration behavior.
    /// </summary>
    public class LegacyPathProvider : IWkhtmltopdfPathProvider
    {
        private readonly string _rotativaLocation;

        /// <summary>
        ///     Constructs path from <see cref="AppDomain.CurrentDomain" />'s base directory,
        ///     <see cref="wkhtmltopdfRelativePath" />,
        ///     <para />
        ///     OS dependent folder and executable name.
        /// </summary>
        /// <param name="wkhtmltopdfRelativePath"></param>
        public LegacyPathProvider(string wkhtmltopdfRelativePath = "Rotativa")
        {
            var wkhtmlPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, wkhtmltopdfRelativePath);

            if (!Directory.Exists(wkhtmlPath))
            {
                throw new Exception("Folder containing wkhtmltopdf not found, searched for " + wkhtmlPath);
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                _rotativaLocation = Path.Combine(wkhtmlPath, "Windows", "wkhtmltopdf.exe");
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                _rotativaLocation = Path.Combine(wkhtmlPath, "Mac", "wkhtmltopdf");
            }
            else
            {
                _rotativaLocation = Path.Combine(wkhtmlPath, "Linux", "wkhtmltopdf");
            }

            if (!File.Exists(_rotativaLocation))
            {
                throw new Exception("wkhtmltopdf not found, searched for " + _rotativaLocation);
            }
        }

        /* <inheritDoc /> */
        public string GetPath() => _rotativaLocation;
    }
}