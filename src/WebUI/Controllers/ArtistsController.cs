using bejebeje.admin.Application.Artists.Commands.CreateArtist;
using bejebeje.admin.Application.Artists.Commands.UpdateArtist;
using bejebeje.admin.Application.Artists.Queries.GetArtists;
using bejebeje.admin.Application.Common.Models;
using bejebeje.admin.Application.TodoLists.Commands.DeleteTodoList;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace bejebeje.admin.WebUI.Controllers;

[Authorize]
public class ArtistsController : CustomControllerBase
{
    [HttpGet]
    public async Task<ActionResult<ArtistsViewModel>> Index()
    {
        PaginatedList<ArtistDto> viewModel = await Mediator.Send(new GetArtistsWithPaginationQuery());
        
        return View(viewModel);
    }

    [HttpPost]
    public async Task<ActionResult<int>> Create(CreateArtistCommand command)
    {
        return await Mediator.Send(command);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, UpdateArtistCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest();
        }

        await Mediator.Send(command);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        await Mediator.Send(new DeleteTodoListCommand { Id = id });

        return NoContent();
    }
}
