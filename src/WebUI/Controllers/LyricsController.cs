﻿using bejebeje.admin.Application.Common.Models;
using bejebeje.admin.Application.Lyrics.Commands.CreateLyric;
using bejebeje.admin.Application.Lyrics.Commands.DeleteLyric;
using bejebeje.admin.Application.Lyrics.Commands.UpdateLyric;
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
    public async Task<ActionResult<PaginatedList<LyricDto>>> Unapproved([FromQuery] GetUnapprovedLyricsWithPaginationQuery query)
    {
        PaginatedList<LyricDto> viewModel = await Mediator.Send(query);
        
        return View(viewModel);
    }
    
    [HttpGet]
    public async Task<ActionResult<PaginatedList<LyricDto>>> Deleted([FromQuery] GetDeletedLyricsWithPaginationQuery query)
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

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, UpdateLyricCommand command)
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
        await Mediator.Send(new DeleteLyricCommand { Id = id });

        return NoContent();
    }
}
