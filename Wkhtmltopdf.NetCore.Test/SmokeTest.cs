using System;
using NUnit.Framework;

namespace Wkhtmltopdf.NetCore.Test
{
    public class SmokeTest
    {
        [Test]
        public void CanConvert()
        {
            WkhtmltopdfConfiguration.RotativaPath = AppDomain.CurrentDomain.BaseDirectory + "Rotativa";
            var generatePdf = new GeneratePdf(null);
            generatePdf.GetPDF("<p><h1>Hello World</h1>This is html rendered text</p>");
        }
    }
}