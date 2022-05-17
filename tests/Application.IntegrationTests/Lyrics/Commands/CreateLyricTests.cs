using bejebeje.admin.Application.Artists.Commands.CreateArtist;
using bejebeje.admin.Application.Common.Exceptions;
using bejebeje.admin.Application.Lyrics.Commands.CreateLyric;
using bejebeje.admin.Domain.Entities;
using FluentAssertions;
using NUnit.Framework;

namespace bejebeje.admin.Application.IntegrationTests.Lyrics.Commands;

using static Testing;

public class CreateLyricTests : TestBase
{
    [Test]
    public async Task ShouldRequireMinimumFields()
    {
        var command = new CreateLyricCommand();

        await FluentActions.Invoking(() =>
            SendAsync(command)).Should().ThrowAsync<ValidationException>();
    }

    [Test]
    public async Task ShouldCreateTodoItem()
    {
        var artistId = await SendAsync(new CreateArtistCommand
        {
            FirstName = "New List"
        });

        var command = new CreateLyricCommand
        {
            ArtistId = artistId,
            Title = "Tasks"
        };

        var itemId = await SendAsync(command);

        var item = await FindAsync<Lyric>(itemId);

        item.Should().NotBeNull();
        item!.ArtistId.Should().Be(command.ArtistId);
        item.Title.Should().Be(command.Title);
        item.CreatedAt.Should().BeCloseTo(DateTime.Now, TimeSpan.FromMilliseconds(10000));
        item.ModifiedAt.Should().BeNull();
    }
}
