using bejebeje.admin.Application.Authors.Queries.GetAuthors;
using bejebeje.admin.Application.Common.Models;
using bejebeje.admin.WebUI.ViewModels.Authors.Queries.GetAuthors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace bejebeje.admin.WebUI.Controllers;

[Authorize]
public class AuthorsController : CustomControllerBase
{
    [HttpGet]
    public async Task<ActionResult<AuthorsViewModel>> All([FromQuery] GetAllAuthorsWithPaginationQuery query)
    {
        PaginatedList<AuthorDto> viewModel = await Mediator.Send(query);

        return View(viewModel);
    }
}