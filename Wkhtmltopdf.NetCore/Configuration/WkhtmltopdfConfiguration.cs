using System;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Wkhtmltopdf.NetCore
{
    public static class WkhtmltopdfConfiguration
    {
        [Obsolete] public static string RotativaPath { get; set; }

        /// <summary>
        ///     Setup Rotativa library
        /// </summary>
        /// <param name="services">The IServiceCollection object</param>
        /// <param name="wkhtmltopdfRelativePath">
        ///     Optional. Relative path to the directory containing wkhtmltopdf.
        ///     Default is "Rotativa". Download at https://wkhtmltopdf.org/downloads.html
        /// </param>
        [Obsolete]
        public static IServiceCollection AddWkhtmltopdf(this IServiceCollection services,
            string wkhtmltopdfRelativePath = "Rotativa")
        {
#pragma warning disable 612
            RotativaPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, wkhtmltopdfRelativePath);

            if (!Directory.Exists(RotativaPath))
                throw new Exception("Folder containing wkhtmltopdf not found, searched for " + RotativaPath);
#pragma warning restore 612

            var fileProvider = new UpdateableFileProvider();
            services.AddMvc()
                .AddRazorRuntimeCompilation(options => options.FileProviders.Add(fileProvider))
                .SetCompatibilityVersion(CompatibilityVersion.Latest);
            return AddCore(services, fileProvider);
        }

        /// <summary>
        ///     Setup Rotativa library.
        /// </summary>
        /// <param name="builder">The <see cref="IMvcBuilder" />.</param>
        /// <param name="lifetime">
        ///     The <see cref="ServiceLifetime" /> of the <see cref="IWkhtmltopdfPathProvider" />.
        /// </param>
        /// <returns>The <see cref="IMvcBuilder" />.</returns>
        public static IMvcBuilder AddWkhtmltopdf<T>(
            this IMvcBuilder builder,
            ServiceLifetime lifetime = ServiceLifetime.Singleton
        ) where T : class, IWkhtmltopdfPathProvider
        {
            return AddWkhtmltopdfInternal<T>(builder, null, lifetime);
        }

        /// <summary>
        ///     Setup Rotativa library.
        /// </summary>
        /// <param name="builder">The <see cref="IMvcBuilder" />.</param>
        /// <param name="factory">A factory used for creating service instances.</param>
        /// <param name="lifetime">
        ///     The <see cref="ServiceLifetime" /> of the <see cref="IWkhtmltopdfPathProvider" />.
        /// </param>
        /// <returns>The <see cref="IMvcBuilder" />.</returns>
        public static IMvcBuilder AddWkhtmltopdf<T>(
            this IMvcBuilder builder,
            Func<IServiceProvider, T> factory,
            ServiceLifetime lifetime = ServiceLifetime.Singleton
        ) where T : class, IWkhtmltopdfPathProvider
        {
            return AddWkhtmltopdfInternal(builder, factory, lifetime);
        }

        private static IMvcBuilder AddWkhtmltopdfInternal<T>(
            IMvcBuilder builder,
            Func<IServiceProvider, T> factory,
            ServiceLifetime lifetime)
            where T : class, IWkhtmltopdfPathProvider
        {
            builder.Services.TryAdd(factory == null
                ? new ServiceDescriptor(typeof(IWkhtmltopdfPathProvider), typeof(T), lifetime)
                : new ServiceDescriptor(typeof(IWkhtmltopdfPathProvider), factory, lifetime));

            var fileProvider = new UpdateableFileProvider();
            AddCore(builder.Services, fileProvider);
            return builder;
        }

        private static IServiceCollection AddCore(IServiceCollection services, UpdateableFileProvider fileProvider)
        {
            services.TryAddSingleton(fileProvider);
            services.TryAddTransient<ITempDataProvider, SessionStateTempDataProvider>();
            services.TryAddSingleton<IRazorViewEngine, RazorViewEngine>();
            services.TryAddTransient<IRazorViewToStringRenderer, RazorViewToStringRenderer>();
            services.TryAddTransient<IGeneratePdf, GeneratePdf>();

            return services;
        }
    }
}