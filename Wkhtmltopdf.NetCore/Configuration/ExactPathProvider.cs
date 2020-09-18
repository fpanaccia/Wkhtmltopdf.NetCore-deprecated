namespace Wkhtmltopdf.NetCore
{
    /// <summary>
    ///     Provides exact specified path to wkthmltopdf/wkthmltoimage.
    /// </summary>
    public class ExactPathProvider : IWkhtmltopdfPathProvider
    {
        private readonly string _path;

        /// <summary>
        ///     Constructs new instance of <see cref="ExactPathProvider" />. Uses provided path as is.
        /// </summary>
        /// <param name="path">Path to wkthmltopdf/wkthmltoimage.</param>
        public ExactPathProvider(string path = "wkhtmltopdf")
        {
            _path = path;
        }

        /** <inheritDoc /> */
        public string GetPath() => _path;
    }
}
