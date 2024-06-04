using bejebeje.admin.Application.Artists.Commands.ApproveArtist;
using bejebeje.admin.Application.Artists.Commands.CreateArtist;
using bejebeje.admin.Application.Artists.Commands.DeleteArtist;
using bejebeje.admin.Application.Artists.Commands.UnapproveArtist;
using bejebeje.admin.Application.Artists.Commands.UndeleteArtist;
using bejebeje.admin.Application.Artists.Commands.UpdateArtist;
using bejebeje.admin.Application.Artists.Queries.GetArtist;
using bejebeje.admin.Application.Artists.Queries.GetArtists;
using bejebeje.admin.Application.Common.Models;
using bejebeje.admin.WebUI.ViewModels.Artists.Queries.GetArtist;
using bejebeje.admin.WebUI.ViewModels.Artists.Queries.GetArtists;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ArtistDto = bejebeje.admin.Application.Artists.Queries.GetArtists.ArtistDto;

namespace bejebeje.admin.WebUI.Controllers;

[Authorize]
public class ArtistsController : CustomControllerBase
{
    [HttpGet]
    public async Task<ActionResult<ArtistsViewModel>> All(
        [FromQuery] GetAllArtistsWithPaginationQuery query)
    {
        PaginatedList<ArtistDto> viewModel = await Mediator.Send(query);

        return View(viewModel);
    }

    [HttpGet]
    public async Task<ActionResult<ArtistsViewModel>> Unapproved(
        string searchTerm,
        int pageNumber = 1,
        int pageSize = 10)
    {
        PaginatedList<ArtistDto> viewModel =
            await Mediator
                .Send(new GetUnapprovedArtistsWithPaginationQuery(searchTerm, pageNumber, pageSize));

        return View(viewModel);
    }

    [HttpGet]
    public async Task<ActionResult<ArtistsViewModel>> Duplicates(
        string searchTerm,
        int pageNumber = 1,
        int pageSize = 10)
    {
        PaginatedList<ArtistDto> viewModel =
            await Mediator
                .Send(new GetDuplicateArtistsWithPaginationQuery(searchTerm, pageNumber, pageSize));

        return View(viewModel);
    }

    [HttpGet]
    public async Task<ActionResult<ArtistsViewModel>> Deleted(
        string searchTerm,
        int pageNumber = 1,
        int pageSize = 10)
    {
        PaginatedList<ArtistDto> viewModel =
            await Mediator.Send(new GetDeletedArtistsWithPaginationQuery(searchTerm, pageNumber, pageSize));

        return View(viewModel);
    }

    [HttpGet]
    public async Task<ActionResult<ArtistViewModel>> Details(
        int artistId)
    {
        Application.Artists.Queries.GetArtist.ArtistDto viewModel = await Mediator
            .Send(new GetArtistQuery(artistId));

        return View(viewModel);
    }

    [HttpGet]
    public async Task<ActionResult<int>> Create(CreateArtistQuery query)
    {
        var viewModel = await Mediator.Send(query);

        return View("Create", viewModel);
    }

    [HttpPost]
    public async Task<ActionResult<int>> Create(CreateArtistCommand command)
    {
        int artistId = await Mediator.Send(command);

        return RedirectToAction("Details", new { artistId = artistId });
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(
        int id,
        UpdateArtistCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest();
        }

        await Mediator.Send(command);

        return NoContent();
    }

    [HttpPost]
    public async Task<ActionResult> Delete(
        DeleteArtistCommand command)
    {
        await Mediator.Send(command);

        return RedirectToAction("Details", new { artistId = command.ArtistId });
    }

    [HttpPost]
    public async Task<ActionResult> Undelete(
        UndeleteArtistCommand command)
    {
        await Mediator.Send(command);

        return RedirectToAction("Details", new { artistId = command.ArtistId });
    }

    [HttpPost]
    public async Task<ActionResult> Approve(
        ApproveArtistCommand command)
    {
        await Mediator.Send(command);

        return RedirectToAction("Details", new { artistId = command.ArtistId });
    }

    [HttpPost]
    public async Task<ActionResult> Unapprove(
        UnapproveArtistCommand command)
    {
        await Mediator.Send(command);

        return RedirectToAction("Details", new { artistId = command.ArtistId });
    }
}