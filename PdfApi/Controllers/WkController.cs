using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.Language.Extensions;
using PdfApi.Shared;
using Wkhtmltopdf.NetCore;

namespace PdfApi.Controllers;
[ApiController]
[Route("[controller]")]
public class WkController : ControllerBase
{
    private readonly ILogger<WkController> _logger;
    private readonly IGeneratePdf _generator;

    public WkController(ILogger<WkController> logger, IGeneratePdf generator)
    {
        _logger = logger;
        _generator = generator;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] WkPdfRequest request)
    {
        try
        {
            ConvertOptions convertOptions = new()
            {
                PageSize = (Wkhtmltopdf.NetCore.Options.Size?)request.PageSize,
                PageWidth = request.PageWidth,
                PageHeight = request.PageHeight,
                PageOrientation = (Wkhtmltopdf.NetCore.Options.Orientation?)request.PageOrientation,
                PageMargins = new()
                {
                    Top = request.PageMargins.Top,
                    Bottom = request.PageMargins.Bottom,
                    Left = request.PageMargins.Left,
                    Right = request.PageMargins.Right,
                },
                EnableForms = request.EnableForms,
                IsLowQuality = request.IsLowQuality,
                Copies = request.Copies,
                ImageDpi = request.ImageDpi,
                ImageQuality = request.ImageQuality,
                Dpi = request.Dpi,
                IsGrayScale = request.IsGrayScale,
                PrintMediaType = request.PrintMediaType,
                DisableSmartShrinking = request.DisableSmartShrinking,
                NoOutline = request.NoOutline,
                DisableExternalLinks = request.DisableExternalLinks,
                DisableInternalLinks = request.DisableInternalLinks,
                NoBackground = request.NoBackground,
                FooterLine = request.FooterLine,
                HeaderHtml = request.HeaderHtml,
                HeaderSpacing = request.HeaderSpacing,
                FooterHtml = request.FooterHtml,
                FooterCenter = request.FooterCenter,
                FooterLeft = request.FooterLeft,
                FooterSpacing = request.FooterSpacing,
                Replacements = request.Replacements
            };

            _generator.SetConvertOptions(convertOptions);
            var pdf = await _generator.GetPdf(new Uri(request.Url));
            if (pdf == null)
                return BadRequest("Não foi possível gerar o PDF.");

            return pdf;
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex.Message, ex);
            return base.BadRequest("Erro crítico: não foi possível gerar o PDF.");
        }
    }
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        try
        {
            ConvertOptions convertOptions = new()
            {
                              FooterRight = "[page]/[toPage]",
                FooterLeft = "[datetime]",
                Replacements = new Dictionary<string, string>
                {
                    {"datetime", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")}
                },
                PageMargins = new ()
                {
                    Right = 10,
                    Left = 4
                },
                FooterSpacing = 3,
                IsLowQuality = true,
                NoOutline = true,
                ImageDpi = 300,
                DisableExternalLinks = true,
                DisableInternalLinks = true,
                ImageQuality = 80,
                PrintMediaType = true,
                Dpi = 70,
                FooterLine = true
            };

            _generator.SetConvertOptions(convertOptions);
            var pdf = await _generator.GetPdf(new Uri("https://painel.teorico.com.br/Classes/presenceData/2e337e88-7587-4a70-8adb-0a7b7988be80?key=pdfExportInternalOnlyChangeThisLater&offset=0"));
            if (pdf == null)
                return BadRequest("Não foi possível gerar o PDF.");

            return pdf;
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex.Message, ex);
            return base.BadRequest("Erro crítico: não foi possível gerar o PDF.");
        }
    }
}
