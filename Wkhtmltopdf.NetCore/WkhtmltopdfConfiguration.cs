using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.IO;

namespace Wkhtmltopdf.NetCore
{
    public static class WkhtmltopdfConfiguration
    {
        public static string RotativaPath { get; set; }

        /// <summary>
        /// Setup Rotativa library
        /// </summary>
        /// <param name="services">The IServiceCollection object</param>
        /// <param name="wkhtmltopdfRelativePath">Optional. Relative path to the directory containing wkhtmltopdf. Default is "Rotativa". Download at https://wkhtmltopdf.org/downloads.html</param>
        public static IServiceCollection AddWkhtmltopdf(this IServiceCollection services, string wkhtmltopdfRelativePath = "Rotativa")
        {
            RotativaPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, wkhtmltopdfRelativePath);

            if (!Directory.Exists(RotativaPath))
            {
                throw new Exception("Folder containing wkhtmltopdf not found, searched for " + RotativaPath);
            }

            var fileProvider = new UpdateableFileProvider();
            services.TryAddTransient<ITempDataProvider, SessionStateTempDataProvider>();
            services.TryAddSingleton(fileProvider);
            services.TryAddSingleton<IRazorViewEngine, RazorViewEngine>();
            services.AddMvc().AddRazorRuntimeCompilation(options => options.FileProviders.Add(fileProvider))
                .SetCompatibilityVersion(CompatibilityVersion.Latest);
            services.TryAddTransient<IRazorViewToStringRenderer, RazorViewToStringRenderer>();
            services.TryAddTransient<IGeneratePdf, GeneratePdf>();

            return services;
        }
    }
}
