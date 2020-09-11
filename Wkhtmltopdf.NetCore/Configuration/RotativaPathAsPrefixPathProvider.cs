using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Wkhtmltopdf.NetCore
{
    /// <summary>
    ///     Legacy. Uses <see cref="WkhtmltopdfConfiguration.RotativaPath" /> as prefix.
    ///     <para />
    ///     Appends OS depended folder and executable name.
    /// </summary>
    internal class RotativaPathAsPrefixPathProvider : IWkhtmltopdfPathProvider
    {
        internal static RotativaPathAsPrefixPathProvider Default { get; } = new RotativaPathAsPrefixPathProvider();

        /* <inheritDoc /> */
        public string GetPath()
        {
#pragma warning disable 612
            var wkhtmlPath = WkhtmltopdfConfiguration.RotativaPath;
#pragma warning restore 612

            string rotativaLocation;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                rotativaLocation = Path.Combine(wkhtmlPath, "Windows", "wkhtmltopdf.exe");
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                rotativaLocation = Path.Combine(wkhtmlPath, "Mac", "wkhtmltopdf");
            }
            else
            {
                rotativaLocation = Path.Combine(wkhtmlPath, "Linux", "wkhtmltopdf");
            }

            if (!File.Exists(rotativaLocation))
            {
                throw new Exception("wkhtmltopdf not found, searched for " + rotativaLocation);
            }

            return rotativaLocation;
        }
    }
}
