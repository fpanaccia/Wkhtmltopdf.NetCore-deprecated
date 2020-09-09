using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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

        [Test]
        public void LegacyResolutionWorks()
        {
            using var host = Host.CreateDefaultBuilder()
                .ConfigureWebHostDefaults(webBuilder =>
                {
#pragma warning disable 612
                    webBuilder.ConfigureServices(services => services.AddWkhtmltopdf());
#pragma warning restore 612
                })
                .Build();

            using var serviceScope = host.Services.CreateScope();
            var generatePdf = serviceScope.ServiceProvider.GetService<IGeneratePdf>();
            generatePdf.GetPDF("<p><h1>Hello World</h1>This is html rendered text</p>");
        }

        [Test]
        public void ResolutionWorks()
        {
            using var host = Host.CreateDefaultBuilder()
                .ConfigureWebHostDefaults(builder => {})
                .ConfigureServices(services => services
                    .AddMvc()
                    .AddWkhtmltopdf<LegacyPathProvider>())
                .Build();

            using var serviceScope = host.Services.CreateScope();
            var generatePdf = serviceScope.ServiceProvider.GetService<IGeneratePdf>();
            generatePdf.GetPDF("<p><h1>Hello World</h1>This is html rendered text</p>");
        }
    }
}
