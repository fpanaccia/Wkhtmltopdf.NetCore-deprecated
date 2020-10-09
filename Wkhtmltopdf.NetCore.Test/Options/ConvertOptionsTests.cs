using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text;
using NUnit.Framework;
using Wkhtmltopdf.NetCore.Interfaces;
using Wkhtmltopdf.NetCore.Options;

namespace Wkhtmltopdf.NetCore.Test.Options
{
    public class ConvertOptionsTests
    {
        [Test]
        public void ConvertsEmpty()
        {
            IConvertOptions options = new ConvertOptions();
            Assert.AreEqual(string.Empty, options.GetConvertOptions());
        }
        
        [Test]
        public void ConvertsAll()
        {
            var switches = new List<string>();
            var counter = 0;
            IConvertOptions options = new ConvertOptions
            {
                Copies = AddWithFormat(++counter, "--copies {0}"),
                EnableForms = AddWithValue(true, "--enable-forms"),
                FooterHtml = AddWithFormat(nameof(ConvertOptions.FooterHtml), "--footer-html {0}"),
                FooterSpacing = AddWithFormat(++counter, "--footer-spacing {0}"),
                HeaderHtml = AddWithFormat(nameof(ConvertOptions.HeaderHtml), "--header-html {0}"),
                HeaderSpacing = AddWithFormat(++counter, "--header-spacing {0}"),
                PageHeight = AddWithFormat(++counter + 0.5, "--page-height {0}"),
                PageMargins = AddWithValues(new Margins(1, 2, 3, 4),
                    new[] {"-B 3", "-L 4", "-R 2", "-T 1"}),
                PageOrientation = AddWithFormat(Orientation.Landscape, "-O {0}"),
                PageSize = AddWithFormat(Size.Legal, "-s {0}"),
                PageWidth = AddWithFormat(++counter + 0.5, "--page-width {0}"),
                IsGrayScale = AddWithValue(true, "-g"),
                IsLowQuality = AddWithValue(true, "-l"),
                DisableSmartShrinking = AddWithValue(true, "--disable-smart-shrinking"),
                Replacements = AddWithValues(new Dictionary<string, string>
                    {
                        {"one", "1"},
                        {"two", "2"}
                    },
                    new[] {"--replace \"one\" \"1\"", "--replace \"two\" \"2\""}),
                ImageDpi = AddWithFormat(++counter, "--image-dpi {0}"),
                ImageQuality = AddWithFormat(++counter, "--image-quality {0}")
            };
            
            var result = options.GetConvertOptions();
            
            var allSwitches = typeof(ConvertOptions).GetProperties()
                .Select(p => p.GetCustomAttribute<OptionFlag>())
                .Where(a => a != null)
                .Select(a => a.Name);
            foreach (var @switch in allSwitches)
            {
                StringAssert.Contains(@switch, result, $"Switch \"{@switch}\" was not set up.");
            }

            var sb = new StringBuilder(result);
            foreach (var @switch in switches)
            {
                StringAssert.Contains(@switch, result, $"Should contain \"{@switch}\".");
                sb.Replace(@switch, string.Empty);
            }
            Assert.IsEmpty(sb.ToString().Trim());

            // Local functions

            T AddWithValue<T>(T value, string switchToAdd)
            {
                switches.Add(switchToAdd);
                return value;
            }
            
            T AddWithValues<T>(T value, IEnumerable<string> switchesToAdd)
            {
                switches.AddRange(switchesToAdd);
                return value;
            }
            
            T AddWithFormat<T>(T value, string formatToAdd)
            {
                switches.Add(string.Format(formatToAdd, value));
                return value;
            }
        }

        [Test]
        public void ConvertsChild()
        {
            IConvertOptions options = new CustomConvertOptions
            {
                Test1 = nameof(CustomConvertOptions.Test1),
                Test2 = true,
                Ignored1 = "Ignored",
                Ignored2 = "Ignored",
                Ignored3 = "Ignored"
            };

            var result = options.GetConvertOptions();
            
            Assert.AreEqual($"-t1 {nameof(CustomConvertOptions.Test1)} -t2", result);
        }

        [SuppressMessage("ReSharper", "NotAccessedField.Local")]
        [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Local")]
        private class CustomConvertOptions : ConvertOptions
        {
            [OptionFlag("-t1")] public string Test1 { get; set; }
            [OptionFlag("-t2")] public bool Test2 { get; set; }
            [OptionFlag("-ignr")] public string Ignored1;
            public string Ignored2 { get; set; }
            public string Ignored3;
        }
    }
}
