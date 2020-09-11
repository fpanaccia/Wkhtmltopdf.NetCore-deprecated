namespace Wkhtmltopdf.NetCore
{
    public interface IWkhtmltopdfPathProvider
    {
        /// <summary>
        ///     Gets path to executable.
        /// </summary>
        /// <returns>Path to executable.</returns>
        string GetPath();
    }
}