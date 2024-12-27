using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Hotel.Client.Areas.Admin.PDF
{
    public class RenderViewHelper
    {
        /// <summary>
        /// Render a Partial View to a string.
        /// </summary>
        /// <param name="controller">Controller instance.</param>
        /// <param name="viewName">Partial view name.</param>
        /// <param name="model">Model to bind to the partial view.</param>
        /// <returns>Rendered HTML as a string.</returns>
        public static async Task<string> RenderPartialViewToString(Controller controller, string viewName, object model)
        {
            controller.ViewData.Model = model;

            using (var writer = new StringWriter())
            {
                var viewEngine = controller.HttpContext.RequestServices.GetService(typeof(ICompositeViewEngine)) as ICompositeViewEngine;
                var viewResult = viewEngine.FindView(controller.ControllerContext, viewName, false);

                if (!viewResult.Success)
                {
                    throw new FileNotFoundException($"Partial View {viewName} not found.");
                }

                var viewContext = new ViewContext(
                    controller.ControllerContext,
                    viewResult.View,
                    controller.ViewData,
                    controller.TempData,
                    writer,
                    new HtmlHelperOptions()
                );

                await viewResult.View.RenderAsync(viewContext);
                return writer.GetStringBuilder().ToString();
            }
        }
    }
}
