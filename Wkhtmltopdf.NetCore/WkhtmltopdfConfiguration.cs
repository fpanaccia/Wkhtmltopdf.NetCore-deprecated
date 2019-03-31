using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
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

            var updateableFileProvider = new UpdateableFileProvider();
            var diagnosticSource = new DiagnosticListener("Microsoft.AspNetCore");
            services.TryAddSingleton<ObjectPoolProvider, DefaultObjectPoolProvider>();
            services.TryAddSingleton<DiagnosticSource>(diagnosticSource);
            services.TryAddTransient<ITempDataProvider, SessionStateTempDataProvider>();
            services.TryAddTransient<IRazorViewToStringRenderer, RazorViewToStringRenderer>();
            services.TryAddTransient<IGeneratePdf, GeneratePdf>();
            services.TryAddSingleton(updateableFileProvider);

            if (services.BuildServiceProvider().GetService<IControllerFactory>() == null)
            {
                //services.TryAddTransient<IRazorViewEngine, RazorViewEngine>();
                var _hostingEnvironment = new HostingEnvironment();
                services.TryAddSingleton<IHostingEnvironment>(_hostingEnvironment);
                services.TryAddSingleton<IConfiguration>(new ConfigurationBuilder()
                                        .SetBasePath(Directory.GetCurrentDirectory())
                                        .Build());
                services.AddLogging();
                services.AddMvc();
            }
            services.Configure<RazorViewEngineOptions>(options =>
            {
                options.FileProviders.Add(updateableFileProvider);
            });

            return services;
        }
    }
}
