namespace Wkhtmltopdf.NetCore.Interfaces
{
    public interface IConvertOptions
    {
        /// <summary>
        /// Builds conversion options. 
        /// </summary>
        /// <returns>Options for executable.</returns>
        public string GetConvertOptions();
    }
}
