using bejebeje.admin.Application.Artists.Commands.CreateArtist;
using bejebeje.admin.Application.Common.Exceptions;
using bejebeje.admin.Application.Lyrics.Commands.CreateLyric;
using bejebeje.admin.Application.Lyrics.Commands.UpdateLyric;
using bejebeje.admin.Domain.Entities;
using FluentAssertions;
using NUnit.Framework;

namespace bejebeje.admin.Application.IntegrationTests.Lyrics.Commands;

using static Testing;

public class UpdateLyricTests : TestBase
{
    [Test]
    public async Task ShouldRequireValidTodoItemId()
    {
        var command = new UpdateLyricCommand { LyricId = 99, Title = "New Title" };
        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<NotFoundException>();
    }

    [Test]
    public async Task ShouldUpdateTodoItem()
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

        var command = new UpdateLyricCommand
        {
            LyricId = itemId,
            Title = "Updated Item Title"
        };

        await SendAsync(command);

        var item = await FindAsync<Lyric>(itemId);

        item.Should().NotBeNull();
        item!.Title.Should().Be(command.Title);
        item.ModifiedAt.Should().NotBeNull();
        item.ModifiedAt.Should().BeCloseTo(DateTime.Now, TimeSpan.FromMilliseconds(10000));
    }
}
