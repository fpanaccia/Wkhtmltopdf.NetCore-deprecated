namespace Wkhtmltopdf.NetCore
{
    /// <summary>
    /// Provides path to wkhtmltopdf.
    /// </summary>
    public interface IWkhtmltopdfPathProvider
    {
        /// <summary>
        ///     Gets path to executable.
        /// </summary>
        /// <returns>Path to executable.</returns>
        string GetPath();
    }
}
