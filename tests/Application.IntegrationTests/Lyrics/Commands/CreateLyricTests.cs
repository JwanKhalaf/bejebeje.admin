using bejebeje.admin.Application.Artists.Commands.CreateArtist;
using bejebeje.admin.Application.Common.Exceptions;
using bejebeje.admin.Application.Lyrics.Commands.CreateLyric;
using Bejebeje.Shared.Domain;
using NUnit.Framework;
using Shouldly;

namespace bejebeje.admin.Application.IntegrationTests.Lyrics.Commands;

using static Testing;

public class CreateLyricTests : TestBase
{
    [Test]
    public async Task ShouldRequireMinimumFields()
    {
        var command = new CreateLyricCommand();

        await Should.ThrowAsync<ValidationException>(() => SendAsync(command));
    }

    [Test]
    public async Task ShouldCreateTodoItem()
    {
        var artistId = await SendAsync(new CreateArtistCommand
        {
            FirstName = "New List",
            Sex = "m"
        });

        var command = new CreateLyricCommand
        {
            ArtistId = artistId,
            Title = "Tasks",
            Body = "some lyric body",
            YouTubeLink = "https://youtu.be/example"
        };

        await SendAsync(command);

        (await CountAsync<Lyric>()).ShouldBe(1);
    }
}
