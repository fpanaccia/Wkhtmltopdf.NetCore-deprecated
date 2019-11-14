using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.IO;

namespace Wkhtmltopdf.NetCore
{
    public static class WkhtmltopdfConfiguration
    {
        public static string RuntimePath { get; set; }

        /// <summary>
        /// Setup Runtime library
        /// </summary>
        /// <param name="services">The IServiceCollection object</param>
        /// <param name="wkhtmltopdfRuntimePath">Optional. Relative path to the directory containing wkhtmltopdf. Default is "runtimes". Download at https://wkhtmltopdf.org/downloads.html</param>
        public static IServiceCollection AddWkhtmltopdf(this IServiceCollection services, string wkhtmltopdfRuntimePath = "runtimes")
        {
            RuntimePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, wkhtmltopdfRuntimePath);

            if (!Directory.Exists(RuntimePath))
            {
                throw new Exception("Folder containing wkhtmltopdf not found, searched for " + RuntimePath);
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