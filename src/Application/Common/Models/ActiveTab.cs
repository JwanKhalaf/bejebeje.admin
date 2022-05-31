namespace bejebeje.admin.Application.Common.Models;

public class ActiveTab
{
    public ActiveTab(string controller, string action, string tabTitle)
    {
        Controller = controller;
        Action = action;
        TabTitle = tabTitle;
    }

    public string Controller { get; set; }

    public string Action { get; set; }
    
    public string TabTitle { get; set; }
}