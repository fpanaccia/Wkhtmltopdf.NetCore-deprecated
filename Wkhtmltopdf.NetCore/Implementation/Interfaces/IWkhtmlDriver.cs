using Wkhtmltopdf.NetCore.Interfaces;

namespace Wkhtmltopdf.NetCore
{
    /// <summary>
    /// Invokes the wkhtmltopdf.
    /// </summary>
    public interface IWkhtmlDriver
    {
        /// <summary>
        /// Converts given URL or HTML string to PDF.
        /// </summary>
        /// <param name="options"><see cref="IConvertOptions"/> that will be passed to wkhtmltopdf binary.</param>
        /// <param name="html">String containing HTML code that should be converted to PDF.</param>
        /// <returns>PDF as byte array.</returns>
        byte[] Convert(IConvertOptions options, string html);
    }
}
