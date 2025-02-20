using bejebeje.admin.Application.Authors.Queries.GetAuthor;
using bejebeje.admin.Application.Authors.Queries.GetAuthors;
using bejebeje.admin.Application.Common.Models;
using bejebeje.admin.WebUI.ViewModels.Authors.Queries.GetAuthor;
using bejebeje.admin.WebUI.ViewModels.Authors.Queries.GetAuthors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AuthorDto = bejebeje.admin.Application.Authors.Queries.GetAuthor.AuthorDto;

namespace bejebeje.admin.WebUI.Controllers;

[Authorize]
public class AuthorsController : CustomControllerBase
{
    [HttpGet]
    public async Task<ActionResult<AuthorsViewModel>> All([FromQuery] GetAllAuthorsWithPaginationQuery query)
    {
        PaginatedList<Application.Authors.Queries.GetAuthors.AuthorDto> viewModel = await Mediator.Send(query);

        return View(viewModel);
    }
    
    [HttpGet]
    public async Task<ActionResult<AuthorViewModel>> Details(int authorId)
    {
        AuthorDto viewModel = await Mediator.Send(new GetAuthorQuery(authorId));

        return View(viewModel);
    }
}