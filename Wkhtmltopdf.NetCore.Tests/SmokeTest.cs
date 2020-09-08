using System;
using NUnit.Framework;
using Wkhtmltopdf.NetCore;

namespace Gemini.Wkhtmltopdf.NetCore
{
    public class SmokeTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void CanGenerate()
        {
            WkhtmltopdfConfiguration.RotativaPath = AppDomain.CurrentDomain.BaseDirectory + "Rotativa";
            var generatePdf = new GeneratePdf(null);
            generatePdf.GetPDF("<p><h1>Hello World</h1>This is html rendered text</p>");
        }
    }
}