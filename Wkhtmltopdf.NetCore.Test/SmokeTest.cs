using System;
using NUnit.Framework;

namespace Wkhtmltopdf.NetCore.Test
{
    public class SmokeTest
    {
        [SetUp]
        public void Setup()
        {
#pragma warning disable 612
            WkhtmltopdfConfiguration.RotativaPath = AppDomain.CurrentDomain.BaseDirectory + "Rotativa";
#pragma warning restore 612
        }

        [Test]
        public void CanConvert()
        {
            var generatePdf = new GeneratePdf(null);
            generatePdf.GetPDF("<p><h1>Hello World</h1>This is html rendered text</p>");
        }
        
        [Test]
        public void CanConvertWithLegacyProvider()
        {
            var generatePdf = new GeneratePdf(null, new LegacyPathProvider());
            generatePdf.GetPDF("<p><h1>Hello World</h1>This is html rendered text</p>");
        }
        
        [Test]
        public void CanConvertWithAbsoluteProvider()
        {
            var path = new LegacyPathProvider().GetPath();
            var generatePdf = new GeneratePdf(null, new AbsolutePathProvider(path));
            generatePdf.GetPDF("<p><h1>Hello World</h1>This is html rendered text</p>");
        }
    }
}
