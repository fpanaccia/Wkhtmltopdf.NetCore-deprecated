using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Wkhtmltopdf.NetCore
{
    public abstract class WkhtmlDriver
    {
        /// <summary>
        /// Converts given URL or HTML string to PDF.
        /// </summary>
        /// <param name="wkhtmlPath">Path to wkthmltopdf\wkthmltoimage.</param>
        /// <param name="switches">Switches that will be passed to wkhtmltopdf binary.</param>
        /// <param name="url">Path to the url that should be converted to PDF.</param>
        /// <returns>PDF as byte array.</returns>
        public static async Task<byte[]> Convert(string wkhtmlPath, string switches, Uri url)
        {
            // switches:
            //     "-q"  - silent output, only errors - no progress messages
            //     " -"  - switch output to stdout
            //     "- -" - switch input to stdin and output to stdout

            var file = Path.GetTempFileName();
            switches = $"-q {switches} \"{url}\" \"{file}\"";

            string rotativaLocation;

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                rotativaLocation = Path.Combine(wkhtmlPath, "Windows", "wkhtmltopdf.exe");
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                rotativaLocation = Path.Combine(wkhtmlPath, "Mac", "wkhtmltopdf");
            }
            else
            {
                rotativaLocation = Path.Combine(wkhtmlPath, "wkhtmltopdf");
            }

            if (!File.Exists(rotativaLocation))
            {
                throw new Exception("wkhtmltopdf not found, searched for " + rotativaLocation);
            }

            using (var proc = new Process())
            {
                try
                {
                    proc.StartInfo = new ProcessStartInfo
                    {
                        FileName = rotativaLocation,
                        Arguments = switches,
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        RedirectStandardInput = true,
                        CreateNoWindow = true
                    };

                    proc.Start();
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                await proc.WaitForExitAsync();

                return await File.ReadAllBytesAsync(file);
            }
        }

        /// <summary>
        /// Converts given URL or HTML string to PDF.
        /// </summary>
        /// <param name="wkhtmlPath">Path to wkthmltopdf\wkthmltoimage.</param>
        /// <param name="switches">Switches that will be passed to wkhtmltopdf binary.</param>
        /// <param name="html">String containing HTML code that should be converted to PDF.</param>
        /// <returns>PDF as byte array.</returns>
        public static async Task<byte[]> Convert(string wkhtmlPath, string switches, string html)
        {
            string rotativaLocation;

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                rotativaLocation = Path.Combine(wkhtmlPath, "Windows", "wkhtmltopdf.exe");
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                rotativaLocation = Path.Combine(wkhtmlPath, "Mac", "wkhtmltopdf");
            }
            else
            {
                rotativaLocation = Path.Combine(wkhtmlPath, "wkhtmltopdf");
            }

            if (!File.Exists(rotativaLocation))
            {
                throw new Exception("wkhtmltopdf not found, searched for " + rotativaLocation);
            }
            
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) || RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                // switches:
                //     "-q"  - silent output, only errors - no progress messages
                //     " -"  - switch output to stdout
                //     "- -" - switch input to stdin and output to stdout
                switches = "-q " + switches + " -";

                // generate PDF from given HTML string, not from URL
                if (!string.IsNullOrEmpty(html))
                {
                    switches += " -";
                    html = SpecialCharsEncode(html);
                }

                using (var proc = new Process())
                {
                    proc.StartInfo = new ProcessStartInfo
                    {
                        FileName = rotativaLocation,
                        Arguments = switches,
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        RedirectStandardInput = true,
                        CreateNoWindow = true
                    };

                    proc.Start();

                    // generate PDF from given HTML string, not from URL
                    if (!string.IsNullOrEmpty(html))
                    {
                        using (var sIn = proc.StandardInput)
                        {
                            sIn.WriteLine(html);
                        }
                    }

                    using (var ms = new MemoryStream())
                    {
                        using (var sOut = proc.StandardOutput.BaseStream)
                        {
                            byte[] buffer = new byte[4096];
                            int read;

                            while ((read = sOut.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                ms.Write(buffer, 0, read);
                            }
                        }

                        string error = proc.StandardError.ReadToEnd();

                        if (ms.Length == 0)
                        {
                            throw new Exception(error);
                        }

                        proc.WaitForExit();

                        return ms.ToArray();
                    }
                }
            }
            else
            {
                // switches:
                //     "-q"  - silent output, only errors - no progress messages
                //     " -"  - switch output to stdout
                //     "- -" - switch input to stdin and output to stdout
                switches = "-q " + switches;

                // generate PDF from given HTML string, not from URL
                if (!string.IsNullOrEmpty(html))
                {
                    html = SpecialCharsEncode(html);
                }

                var fakeFileName = Guid.NewGuid();
                File.WriteAllText($"{fakeFileName}.html", html);

                switches += $" {fakeFileName}.html {fakeFileName}.pdf";

                using (var proc = new Process())
                {
                    proc.StartInfo = new ProcessStartInfo
                    {
                        FileName = rotativaLocation,
                        Arguments = switches,
                        UseShellExecute = false,
                        //RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        //RedirectStandardInput = true,
                        CreateNoWindow = true
                    };

                    proc.Start();

                    using (var ms = new MemoryStream())
                    {
                        string error = proc.StandardError.ReadToEnd();

                        proc.WaitForExit();

                        using (var fileStream = new FileStream($"{fakeFileName}.pdf", FileMode.Open, FileAccess.Read))
                        {
                            fileStream.CopyTo(ms);
                        }

                        File.Delete($"{fakeFileName}.pdf");
                        File.Delete($"{fakeFileName}.html");

                        if (ms.Length == 0)
                        {
                            throw new Exception(error);
                        }

                        return ms.ToArray();
                    }
                }
            }
        }

        /// <summary>
        /// Encode all special chars
        /// </summary>
        /// <param name="text">Html text</param>
        /// <returns>Html with special chars encoded</returns>
        private static string SpecialCharsEncode(string text)
        {
            var chars = text.ToCharArray();
            var result = new StringBuilder(text.Length + (int) (text.Length * 0.1));

            foreach (var c in chars)
            {
                var value = System.Convert.ToInt32(c);
                if (value > 127)
                    result.AppendFormat("&#{0};", value);
                else
                    result.Append(c);
            }

            return result.ToString();
        }
    }
}