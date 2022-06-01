using bejebeje.admin.Application.Common.Security;
using bejebeje.admin.Application.Dashboard.Queries;
using Microsoft.AspNetCore.Mvc;

namespace bejebeje.admin.WebUI.Controllers;

[Authorize]
public class DashboardController : CustomControllerBase
{
    [HttpGet]
    public async Task<ActionResult<DashboardViewModel>> Index()
    {
       var viewModel = await Mediator.Send(new GetDashboardQuery());
        
        return View(viewModel);
    }
}