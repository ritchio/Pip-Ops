using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc;

public static class RenderizarVistaExtension
{
    public static string RenderizarVista<TModel>(this Controller controller, string viewName, TModel model, bool partial = false)
    {
        controller.ViewData.Model = model;

        using var sw = new StringWriter();
        var viewEngine = controller.HttpContext.RequestServices.GetService<ICompositeViewEngine>();
        var viewResult = partial
            ? viewEngine.FindView(controller.ControllerContext, viewName, false)
            : viewEngine.GetView(null, viewName, false);

        if (!viewResult.Success)
            throw new InvalidOperationException($"No se pudo encontrar la vista '{viewName}'.");

        var viewContext = new ViewContext(
            controller.ControllerContext,
            viewResult.View,
            controller.ViewData,
            controller.TempData,
            sw,
            new HtmlHelperOptions()
        );

        viewResult.View.RenderAsync(viewContext);
        return sw.ToString();
    }
}
