using System;

namespace Wkhtmltopdf.NetCore
{
    [Serializable]
    public class WkhtmlDriverException : Exception
    {
        public WkhtmlDriverException(string message, Exception e) : base(message, e)
        {
        }
    }
}