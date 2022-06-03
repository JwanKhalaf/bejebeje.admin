using Microsoft.AspNetCore.Mvc.Rendering;

namespace bejebeje.admin.WebUI.Helpers;

public static class HtmlHelperExtensions
{
    private const string ActiveNavLinkCssClasses =
        "bg-slate-900 before:absolute after:absolute before:-top-12 before:right-0 before:h-12 before:w-12 before:rounded-br-2xl before:shadow-[0_1rem_0_0_rgba(15,23,42,1)] before:shadow-slate-900 after:-bottom-12 after:right-0 after:h-12 after:w-12 after:rounded-tr-2xl after:shadow-[0_-1rem_0_0_rgba(15,23,42,1)] after:shadow-slate-900";

    public static string IsActiveNavLink(this IHtmlHelper htmlHelper, string controller)
    {
        bool returnActive = IsActiveController(htmlHelper, controller);

        return returnActive ? ActiveNavLinkCssClasses : "";
    }

    public static bool IsActiveRoute(this IHtmlHelper htmlHelper, string controller, string action)
    {
        RouteData routeData = htmlHelper.ViewContext.RouteData;

        string routeController = routeData.Values["controller"].ToString().ToLower();
        string routeAction = routeData.Values["action"].ToString().ToLower();

        bool returnActive = controller.ToLower() == routeController && action.ToLower() == routeAction;
        
        return returnActive;
    }
    
    private static bool IsActiveController(this IHtmlHelper htmlHelper, string controller)
    {
        RouteData routeData = htmlHelper.ViewContext.RouteData;

        string routeController = routeData.Values["controller"].ToString().ToLower();

        bool returnActive = controller.ToLower() == routeController;
        
        return returnActive;
    }
}

