using Microsoft.AspNetCore.Mvc.Rendering;

namespace Pisciculture_dotnet2.Helpers;

public static class ViewHelpers
{
    public static string IsActive(this IHtmlHelper htmlHelper, string controller, string action = null)
    {
        var routeData = htmlHelper.ViewContext.RouteData;
        var routeController = routeData.Values["Controller"]?.ToString();
        var routeAction = routeData.Values["Action"]?.ToString();

        var isControllerMatch = string.Equals(controller, routeController, StringComparison.OrdinalIgnoreCase);
        var isActionMatch = action == null || string.Equals(action, routeAction, StringComparison.OrdinalIgnoreCase);

        return isControllerMatch && isActionMatch ? "active" : "";
    }
}