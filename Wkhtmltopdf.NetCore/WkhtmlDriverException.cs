using System;

namespace Wkhtmltopdf.NetCore
{
    /// <summary>
    /// Custom Wkhtmltopdf.NetCore exception.
    /// </summary>
    [Serializable]
    public class WkhtmlDriverException : Exception
    {
        /// <summary>
        /// Constructs new instance of <see cref="WkhtmlDriverException"/>
        /// </summary>
        public WkhtmlDriverException(string message, Exception e) : base(message, e)
        {
        }
    }
}
