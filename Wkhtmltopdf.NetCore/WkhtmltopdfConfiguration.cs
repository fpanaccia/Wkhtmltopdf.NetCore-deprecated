using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.Extensions.ObjectPool;
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

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

            services.TryAddTransient<ITempDataProvider, SessionStateTempDataProvider>();
            services.AddMvc().AddRazorRuntimeCompilation();
            services.TryAddTransient<IRazorViewToStringRenderer, RazorViewToStringRenderer>();
            services.TryAddTransient<IGeneratePdf, GeneratePdf>();

            var mvcBuilder = services.BuildServiceProvider().GetService<IMvcCoreBuilder>();
            if (mvcBuilder == null)
            {
                services.AddMvc().AddRazorRuntimeCompilation();
            }
            else
            {
                mvcBuilder.AddRazorRuntimeCompilation();
            }

            services.Configure<MvcRazorRuntimeCompilationOptions>(options =>
            {
                options.FileProviders.Add(new UpdateableFileProvider());
            });

            return services;
        }
    }
}
