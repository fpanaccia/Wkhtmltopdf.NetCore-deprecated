using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Wkhtmltopdf.NetCore
{
    public class HtmlAsPdf : AsPdfResultBase
    {
        public byte[] GetPDF(string html)
        {
            return WkhtmlDriver.Convert(RotativaConfiguration.RotativaPath, this.GetConvertOptions(), html);
        }
    }
}
