using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Wkhtmltopdf.NetCore
{
    public class RazorViewToStringRenderer : IRazorViewToStringRenderer
    {
        private IRazorViewEngine _viewEngine;
        private ITempDataProvider _tempDataProvider;
        private IServiceProvider _serviceProvider;

        public RazorViewToStringRenderer(
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

        public async Task<string> RenderHtmlToStringAsync<TModel>(string html, TModel model)
        {
            UpdateableFileProvider.UpdateContent(html);
            return await RenderViewToStringAsync("/Views/FakeView.cshtml", model);
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

        public void AddView(string path, string viewHTML)
        {
            if (ExistsView(path))
            {
                throw new Exception($"View {path} already exists");
            }

            UpdateableFileProvider.Views.Add($"/Views/{path}.cshtml", new ViewFileInfo(viewHTML));
        }

        public bool ExistsView(string path)
        {
            return UpdateableFileProvider.Views.Any(x => x.Key == $"/Views/{path}.cshtml");
        }

        public void UpdateView(string path, string viewHTML)
        {
            if (ExistsView(path))
            {
                UpdateableFileProvider.UpdateContent(viewHTML, $"/Views/{path}.cshtml");
            }
            else
            {
                throw new Exception($"View {path} does not exists");
            }
        }

        private ActionContext GetActionContext()
        {
            return new ActionContext(new DefaultHttpContext { RequestServices = _serviceProvider }, new RouteData(), new ActionDescriptor());
        }
    }
}
