using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using RazorLight;
using System;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Wkhtmltopdf.NetCore
{
    public class ViewAsPdf : HtmlAsPdf
    {
        private IRazorViewEngine _viewEngine;
        private ITempDataProvider _tempDataProvider;
        private IServiceProvider _serviceProvider;

        public ViewAsPdf(
    IRazorViewEngine viewEngine,
    ITempDataProvider tempDataProvider,
    IServiceProvider serviceProvider)
        {
            _viewEngine = viewEngine;
            _tempDataProvider = tempDataProvider;
            _serviceProvider = serviceProvider;
        }
        public async Task<string> RenderViewToStringAsync<TModel>(string viewName, TModel model)
        {
            var actionContext = GetActionContext();
            var view = FindView(actionContext, viewName);

            using (var output = new StringWriter())
            {
                var viewContext = new ViewContext(
                    actionContext,
                    view,
                    new ViewDataDictionary<TModel>(
                        metadataProvider: new EmptyModelMetadataProvider(),
                        modelState: new ModelStateDictionary())
                    {
                        Model = model
                    },
                    new TempDataDictionary(
                        actionContext.HttpContext,
                        _tempDataProvider),
                    output,
                    new HtmlHelperOptions());

                await view.RenderAsync(viewContext);

                return output.ToString();
            }
        }
        private IView FindView(ActionContext actionContext, string viewName)
        {
            var getViewResult = _viewEngine.GetView(executingFilePath: null, viewPath: viewName, isMainPage: true);
            if (getViewResult.Success)
            {
                return getViewResult.View;
            }

            var findViewResult = _viewEngine.FindView(actionContext, viewName, isMainPage: true);
            if (findViewResult.Success)
            {
                return findViewResult.View;
            }

            var searchedLocations = getViewResult.SearchedLocations.Concat(findViewResult.SearchedLocations);
            var errorMessage = string.Join(
                Environment.NewLine,
                new[] { $"Unable to find view '{viewName}'. The following locations were searched:" }.Concat(searchedLocations)); ;

            throw new InvalidOperationException(errorMessage);
        }

        private ActionContext GetActionContext()
        {
            var httpContext = new DefaultHttpContext();
            httpContext.RequestServices = _serviceProvider;
            return new ActionContext(httpContext, new RouteData(), new ActionDescriptor());
        }




        RazorLightEngine _engine;
        public ViewAsPdf(string viewsPath = null)
        {
            _engine = new RazorLightEngineBuilder()
                            .UseFilesystemProject(viewsPath)
                            .UseMemoryCachingProvider()
                            .Build();
        }

        public ViewAsPdf()
        {
            _engine = new RazorLightEngineBuilder()
                            .UseFilesystemProject(Directory.GetCurrentDirectory())
                            .UseMemoryCachingProvider()
                            .Build();
        }

        public async Task<byte[]> GetByteArray<T>(string View, T model, ExpandoObject viewBag = null)
        {
            try
            {
                var html = await _engine.CompileRenderAsync(View, model, viewBag);
                return base.GetPDF(html);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IActionResult> GetPdf<T>(string View, T model, ExpandoObject viewBag = null)
        {
            var html = await _engine.CompileRenderAsync(View, model, viewBag);
            var byteArray = base.GetPDF(html);
            MemoryStream pdfStream = new MemoryStream();
            pdfStream.Write(byteArray, 0, byteArray.Length);
            pdfStream.Position = 0;
            return new FileStreamResult(pdfStream, "application/pdf");
        }

        public async Task<byte[]> GetByteArrayViewInHtml<T>(string ViewInHtml, T model, ExpandoObject viewBag = null)
        {
            try
            {
                var view = await _engine.CompileRenderAsync("template", ViewInHtml, model, viewBag);
                return base.GetPDF(view);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

    }
}