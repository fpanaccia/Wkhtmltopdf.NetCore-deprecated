namespace Wkhtmltopdf.NetCore
{
    /// <summary>
    ///     Provides absolute path to wkthmltopdf/wkthmltoimage.
    /// </summary>
    public class FixedPathProvider : IWkhtmltopdfPathProvider
    {
        private readonly string _path;

        /// <summary>
        ///     Constructs <see cref="FixedPathProvider" />. Uses provided path unchanged.
        /// </summary>
        /// <param name="path">Path to wkthmltopdf/wkthmltoimage.</param>
        public FixedPathProvider(string path = "wkhtmltopdf")
        {
            _path = path;
        }

        /* <inheritDoc /> */
        public string GetPath() => _path;
    }
}
