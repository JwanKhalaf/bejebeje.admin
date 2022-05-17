using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace bejebeje.admin.WebUI.Controllers;

public abstract class CustomControllerBase : Controller
{
    private ISender _mediator = null!;

    protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();
}
