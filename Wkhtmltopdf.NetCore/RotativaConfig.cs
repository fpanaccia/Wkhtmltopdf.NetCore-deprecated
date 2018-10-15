using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Wkhtmltopdf.NetCore
{
    public static class RotativaConfiguration
    {
        public static string RotativaPath { get; set; }
        public static bool IsWindows { get; set; }

        /// <summary>
        /// Setup Rotativa library
        /// </summary>
        /// <param name="env">The IHostingEnvironment object</param>
        /// <param name="wkhtmltopdfRelativePath">Optional. Relative path to the directory containing wkhtmltopdf. Default is "Rotativa". Download at https://wkhtmltopdf.org/downloads.html</param>
        public static void Setup(string wkhtmltopdfRelativePath = "Rotativa") 
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
        }

    }
}
