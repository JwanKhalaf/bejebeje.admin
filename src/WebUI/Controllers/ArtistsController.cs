using bejebeje.admin.Application.Artists.Commands.CreateArtist;
using bejebeje.admin.Application.Artists.Commands.UpdateArtist;
using bejebeje.admin.Application.Artists.Queries.CreateArtist;
using bejebeje.admin.Application.Artists.Queries.GetArtists;
using bejebeje.admin.Application.Common.Models;
using bejebeje.admin.Application.TodoLists.Commands.DeleteTodoList;
using bejebeje.admin.WebUI.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace bejebeje.admin.WebUI.Controllers;

[Authorize]
public class ArtistsController : CustomControllerBase
{
    [HttpGet]
    public async Task<ActionResult<ArtistsViewModel>> All([FromQuery] GetAllArtistsWithPaginationQuery query)
    {
        PaginatedList<ArtistDto> viewModel = await Mediator.Send(query);
        
        return View(viewModel);
    }
    
    [HttpGet]
    public async Task<ActionResult<ArtistsViewModel>> Unapproved([FromQuery] GetAllArtistsWithPaginationQuery query)
    {
        PaginatedList<ArtistDto> viewModel = await Mediator.Send(query);
        
        return View(viewModel);
    }
    
    [HttpGet]
    public async Task<ActionResult<ArtistsViewModel>> Deleted([FromQuery] GetAllArtistsWithPaginationQuery query)
    {
        PaginatedList<ArtistDto> viewModel = await Mediator.Send(query);
        
        return View(viewModel);
    }
    
    [HttpGet]
    public async Task<ActionResult> Create()
    {
        CreateArtistGenderDto viewModel = await Mediator.Send(new CreateArtistQuery());
        
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
