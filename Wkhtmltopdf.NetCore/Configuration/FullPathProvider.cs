namespace Wkhtmltopdf.NetCore
{
    /// <summary>
    ///     Provides absolute path to wkthmltopdf/wkthmltoimage.
    /// </summary>
    public class FullPathProvider : IWkhtmltopdfPathProvider
    {
        private readonly string _path;

        /// <summary>
        ///     Constructs <see cref="FullPathProvider" />. Uses provided path as is.
        /// </summary>
        /// <param name="path">Path to wkthmltopdf/wkthmltoimage.</param>
        public FullPathProvider(string path = "wkhtmltopdf")
        {
            _path = path;
        }

        /* <inheritDoc /> */
        public string GetPath() => _path;
    }
}
