using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace bejebeje.admin.WebUI.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class CustomControllerBase : ControllerBase
{
    private ISender _mediator = null!;

    protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();
}
