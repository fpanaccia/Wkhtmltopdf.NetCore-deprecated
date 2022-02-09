
using System.ComponentModel.DataAnnotations;

namespace PdfApi.Shared;
public class WkPdfRequest
{
    public WkPdfRequest()
    {
        PageMargins = new Margins();
    }

    [Required]
    public string Url { get; set; }

    /// <summary>
    /// Sets the page size.
    /// </summary>
    public Size? PageSize { get; set; }

    /// <summary>
    /// Sets the page width in mm.
    /// </summary>
    /// <remarks>Has priority over <see cref="PageSize"/> but <see cref="PageHeight"/> has to be also specified.</remarks>
    public double? PageWidth { get; set; }

    /// <summary>
    /// Sets the page height in mm.
    /// </summary>
    /// <remarks>Has priority over <see cref="PageSize"/> but <see cref="PageWidth"/> has to be also specified.</remarks>
    public double? PageHeight { get; set; }

    /// <summary>
    /// Sets the page orientation.
    /// </summary>
    public Orientation? PageOrientation { get; set; }

    /// <summary>
    /// Sets the page margins.
    /// </summary>
    public Margins PageMargins { get; set; }

    /// <summary>
    /// Indicates whether the PDF should be generated with forms.
    /// </summary>
    public bool EnableForms { get; set; }

    /// <summary>
    /// Indicates whether the PDF should be generated in lower quality.
    /// </summary>
    public bool IsLowQuality { get; set; }
    
    /// <summary>
    /// Number of copies to print into the PDF file.
    /// </summary>
    public int? Copies { get; set; }

    /// <summary>
    /// When embedding images scale them down to this dpi (default 600)
    /// </summary>
    public uint? ImageDpi { get; set; }


    /// <summary>
    /// When jpeg compressing images use this quality (default 94)
    /// </summary>
    public uint? ImageQuality { get; set; }


    /// <summary>
    /// When jpeg compressing images use this quality (default 94)
    /// </summary>
    public uint? Dpi { get; set; }

    /// <summary>
    /// Indicates whether the PDF should be generated in grayscale.
    /// </summary>
    public bool IsGrayScale { get; set; }

    /// <summary>
    /// Indicates whether the PDF should be generated in grayscale.
    /// </summary>
    public bool PrintMediaType { get; set; }

    /// <summary>
    /// Disable the intelligent shrinking strategy used by WebKit that makes the pixel/dpi ratio non-constant.
    /// </summary>
    public bool DisableSmartShrinking { get; set; }

    /// <summary>
    /// Do not put an outline into the pdf
    /// </summary>
    public bool NoOutline { get; set; }

    /// <summary>
    /// Do not make links to remote web pages
    /// </summary>
    public bool DisableExternalLinks { get; set; }

    /// <summary>
    /// Do not make local links
    /// </summary>
    public bool DisableInternalLinks { get; set; }

    /// <summary>
    /// Do not print background
    /// </summary>
    public bool NoBackground { get; set; }

    /// <summary>
    /// Display line above the footer
    /// </summary>
    public bool FooterLine { get; set; }

    /// <summary>
    /// Path to header HTML file.
    /// </summary>
    public string? HeaderHtml { get; set; }

    /// <summary>
    /// Sets the header spacing.
    /// </summary>
    public int? HeaderSpacing { get; set; }

    /// <summary>
    /// Path to footer HTML file.
    /// </summary>
    public string? FooterHtml { get; set; }

    /// <summary>
    /// Footer right content.
    /// </summary>
    public string? FooterRight { get; set; }

    /// <summary>
    /// Footer center content.
    /// </summary>
    public string? FooterCenter { get; set; }

    /// <summary>
    /// Footer left content.
    /// </summary>
    public string? FooterLeft { get; set; }

    /// <summary>
    /// Sets the footer spacing.
    /// </summary>
    public int? FooterSpacing { get; set; }

    /// <summary>
    /// Sets the variables to replace in the header and footer html
    /// </summary>
    /// <remarks>Replaces [name] with value in header and footer (repeatable).</remarks>
    public Dictionary<string, string> Replacements { get; set; }

}