using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.ObjectPool;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace Wkhtmltopdf.NetCore
{
    public static class WkhtmltopdfConfiguration
    {
        public static string RotativaPath { get; set; }
        public static bool IsWindows { get; set; }

        /// <summary>
        /// Setup Rotativa library
        /// </summary>
        /// <param name="env">The IHostingEnvironment object</param>
        /// <param name="wkhtmltopdfRelativePath">Optional. Relative path to the directory containing wkhtmltopdf. Default is "Rotativa". Download at https://wkhtmltopdf.org/downloads.html</param>
        public static IServiceCollection AddWkhtmltopdf(this IServiceCollection services, string wkhtmltopdfRelativePath = "Rotativa")
        {
            string applicationName = (Assembly.GetEntryAssembly() ?? Assembly.GetCallingAssembly()).GetName().Name;
            IFileProvider fileProvider = new PhysicalFileProvider(AppDomain.CurrentDomain.BaseDirectory);

            services.AddSingleton<IHostingEnvironment>(new HostingEnvironment
            {
                ApplicationName = applicationName,
                WebRootFileProvider = fileProvider,
            });
            services.Configure<RazorViewEngineOptions>(options =>
            {
                options.FileProviders.Clear();
                options.FileProviders.Add(fileProvider);
            });

            IsWindows = System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
            if (IsWindows)
            {
                RotativaPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, wkhtmltopdfRelativePath);

                if (!Directory.Exists(RotativaPath))
                {
                    throw new Exception("Folder containing wkhtmltopdf.exe not found, searched for " + RotativaPath);
                }
            }
            else
            {
                RotativaPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, wkhtmltopdfRelativePath);

                if (!Directory.Exists(RotativaPath))
                {
                    throw new Exception("Folder containing wkhtmltopdf not found, searched for " + RotativaPath);
                }
            }

            var diagnosticSource = new DiagnosticListener("Microsoft.AspNetCore");
            services.AddSingleton<ObjectPoolProvider, DefaultObjectPoolProvider>();
            services.AddSingleton<DiagnosticSource>(diagnosticSource);
            services.AddTransient<IRazorViewEngine, RazorViewEngine>();
            services.AddTransient<ITempDataProvider, SessionStateTempDataProvider>();
            services.AddTransient<IRazorViewToStringRenderer, RazorViewToStringRenderer>();
            services.AddTransient<IGeneratePdf, GeneratePdf>();
            services.AddLogging();
            services.AddMvc();
            return services;
        }
    }
}
