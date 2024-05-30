using bejebeje.admin.Application.Common.Models;
using bejebeje.admin.Application.Lyrics.Commands.ApproveLyric;
using bejebeje.admin.Application.Lyrics.Commands.CreateLyric;
using bejebeje.admin.Application.Lyrics.Commands.DeleteLyric;
using bejebeje.admin.Application.Lyrics.Commands.UnapproveLyric;
using bejebeje.admin.Application.Lyrics.Commands.UndeleteLyric;
using bejebeje.admin.Application.Lyrics.Commands.UnverifyLyric;
using bejebeje.admin.Application.Lyrics.Commands.UpdateLyric;
using bejebeje.admin.Application.Lyrics.Commands.VerifyLyric;
using bejebeje.admin.Application.Lyrics.Queries.GetLyricDetail;
using bejebeje.admin.Application.Lyrics.Queries.GetLyrics;
using bejebeje.admin.Application.Lyrics.Queries.GetLyricsForArtist;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LyricDto = bejebeje.admin.Application.Lyrics.Queries.GetLyrics.LyricDto;

namespace bejebeje.admin.WebUI.Controllers;

[Authorize]
public class LyricsController : CustomControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PaginatedList<LyricDto>>> All([FromQuery] GetAllLyricsWithPaginationQuery query)
    {
        PaginatedList<LyricDto> viewModel = await Mediator.Send(query);

        return View(viewModel);
    }

    [HttpGet]
    public async Task<ActionResult<PaginatedList<LyricDto>>> Unapproved(
        [FromQuery] GetUnapprovedLyricsWithPaginationQuery query)
    {
        PaginatedList<LyricDto> viewModel = await Mediator.Send(query);

        return View(viewModel);
    }

    [HttpGet]
    public async Task<ActionResult<PaginatedList<LyricDto>>> Deleted(
        [FromQuery] GetDeletedLyricsWithPaginationQuery query)
    {
        PaginatedList<LyricDto> viewModel = await Mediator.Send(query);

        return View(viewModel);
    }

    [HttpGet]
    public async Task<ActionResult<GetLyricsForArtistDto>> ByArtist([FromQuery] GetAllLyricsForArtistQuery query)
    {
        GetLyricsForArtistDto viewModel = await Mediator.Send(query);

        return View(viewModel);
    }

    [HttpPost]
    public async Task<ActionResult<int>> Create(CreateLyricCommand command)
    {
        return await Mediator.Send(command);
    }

    [HttpGet]
    public async Task<ActionResult<GetLyricDetailDto>> Details([FromQuery] GetLyricDetailQuery query)
    {
        GetLyricDetailDto viewModel = await Mediator.Send(query);

        return View(viewModel);
    }

    [HttpGet]
    public async Task<ActionResult> Update(UpdateLyricQuery query)
    {
        var viewModel = await Mediator.Send(query);

        return View("Update", viewModel);
    }

    [HttpPost]
    public async Task<ActionResult> Update(UpdateLyricCommand command)
    {
        await Mediator.Send(command);

        return RedirectToAction("Details", new { lyricId = command.LyricId });
    }

    [HttpPost]
    public async Task<ActionResult> Delete(DeleteLyricCommand command)
    {
        await Mediator.Send(command);

        return RedirectToAction("Details", new { lyricId = command.LyricId });
    }

    [HttpPost]
    public async Task<ActionResult> Undelete(UndeleteLyricCommand command)
    {
        await Mediator.Send(command);

        return RedirectToAction("Details", new { lyricId = command.LyricId });
    }

    [HttpPost]
    public async Task<ActionResult> Verify(VerifyLyricCommand command)
    {
        await Mediator.Send(command);

        return RedirectToAction("Details", new { lyricId = command.LyricId });
    }

    [HttpPost]
    public async Task<ActionResult> Unverify(UnverifyLyricCommand command)
    {
        await Mediator.Send(command);

        return RedirectToAction("Details", new { lyricId = command.LyricId });
    }

    [HttpPost]
    public async Task<ActionResult> Approve(ApproveLyricCommand command)
    {
        await Mediator.Send(command);

        return RedirectToAction("Details", new { lyricId = command.LyricId });
    }

    [HttpPost]
    public async Task<ActionResult> Unapprove(UnapproveLyricCommand command)
    {
        await Mediator.Send(command);

        return RedirectToAction("Details", new { lyricId = command.LyricId });
    }
}