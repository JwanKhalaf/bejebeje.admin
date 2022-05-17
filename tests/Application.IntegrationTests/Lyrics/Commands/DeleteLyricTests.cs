using bejebeje.admin.Application.Artists.Commands.CreateArtist;
using bejebeje.admin.Application.Common.Exceptions;
using bejebeje.admin.Application.Lyrics.Commands.CreateLyric;
using bejebeje.admin.Application.Lyrics.Commands.DeleteLyric;
using bejebeje.admin.Domain.Entities;
using FluentAssertions;
using NUnit.Framework;

namespace bejebeje.admin.Application.IntegrationTests.Lyrics.Commands;

using static Testing;

public class DeleteLyricTests : TestBase
{
    [Test]
    public async Task ShouldRequireValidTodoItemId()
    {
        var command = new DeleteLyricCommand { Id = 99 };

        await FluentActions.Invoking(() =>
            SendAsync(command)).Should().ThrowAsync<NotFoundException>();
    }

    [Test]
    public async Task ShouldDeleteTodoItem()
    {
        var artistId = await SendAsync(new CreateArtistCommand
        {
            FirstName = "New List"
        });

        var itemId = await SendAsync(new CreateLyricCommand
        {
            ArtistId = artistId,
            Title = "New Item"
        });

        await SendAsync(new DeleteLyricCommand
        {
            Id = itemId
        });

        var item = await FindAsync<Lyric>(itemId);

        item.Should().BeNull();
    }
}
