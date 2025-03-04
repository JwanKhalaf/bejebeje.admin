﻿using bejebeje.admin.Application.Common.Models;
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
using bejebeje.admin.Application.LyricSlugs.Commands.CreateLyricSlug;
using bejebeje.admin.Application.LyricSlugs.Commands.DeleteLyricSlug;
using bejebeje.admin.Application.LyricSlugs.Queries.GetLyricSlugs;
using bejebeje.admin.Domain.Exceptions;
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
    public async Task<ActionResult<PaginatedList<LyricDto>>> Duplicates(
        [FromQuery] GetDuplicateLyricsWithPaginationQuery query)
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

    [HttpGet]
    public async Task<ActionResult> Create(CreateLyricQuery query)
    {
        var viewModel = await Mediator.Send(query);

        return View("Create", viewModel);
    }

    [HttpPost]
    public async Task<ActionResult<int>> Create(CreateLyricCommand command)
    {
        await Mediator.Send(command);

        return RedirectToAction("Details", "Artists", new { artistId = command.ArtistId });
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

    [HttpGet]
    public async Task<ActionResult<GetLyricSlugsQueryViewModel>> Slugs(GetLyricSlugsQuery query)
    {
        var viewModel = await Mediator.Send(query);

        return View("Slugs", viewModel);
    }

    [HttpGet]
    public async Task<ActionResult<int>> CreateSlug(CreateLyricSlugQuery query)
    {
        var viewModel = await Mediator.Send(query);

        return View("CreateSlug", viewModel);
    }

    [HttpPost]
    public async Task<ActionResult<int>> CreateSlug(CreateLyricSlugCommand command)
    {
        if (!ModelState.IsValid)
        {
            CreateLyricSlugQueryViewModel viewModel = new CreateLyricSlugQueryViewModel
            {
                LyricId = command.LyricId, Name = command.Name, IsPrimary = command.IsPrimary
            };

            return View("CreateSlug", viewModel);
        }

        try
        {
            await Mediator.Send(command);

            return RedirectToAction("Slugs", new { lyricId = command.LyricId });
        }
        catch (LyricSlugAlreadyExistsException ex)
        {
            ModelState.AddModelError(nameof(command.Name), ex.Message);

            CreateLyricSlugQueryViewModel viewModel = new CreateLyricSlugQueryViewModel
            {
                LyricId = command.LyricId, Name = command.Name, IsPrimary = command.IsPrimary
            };

            return View("CreateSlug", viewModel);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, "An unexpected error occurred. Please try again.");

            CreateLyricSlugQueryViewModel viewModel = new CreateLyricSlugQueryViewModel
            {
                LyricId = command.LyricId, Name = command.Name, IsPrimary = command.IsPrimary
            };

            return View("CreateSlug", viewModel);
        }
    }

    [HttpPost]
    public async Task<ActionResult<int>> DeleteSlug(DeleteLyricSlugCommand command)
    {
        await Mediator.Send(command);

        return RedirectToAction("Slugs", new { lyricId = command.LyricId });
    }
}