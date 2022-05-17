using bejebeje.admin.Application.Artists.Commands.CreateArtist;
using bejebeje.admin.Application.Common.Exceptions;
using bejebeje.admin.Domain.Entities;
using FluentAssertions;
using NUnit.Framework;

namespace bejebeje.admin.Application.IntegrationTests.Artists.Commands;

using static Testing;

public class CreateArtistsTests : TestBase
{
    [Test]
    public async Task ShouldRequireMinimumFields()
    {
        var command = new CreateArtistCommand();
        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<ValidationException>();
    }

    [Test]
    public async Task ShouldRequireUniqueTitle()
    {
        await SendAsync(new CreateArtistCommand
        {
            FirstName = "Shopping"
        });

        var command = new CreateArtistCommand
        {
            FirstName = "Shopping"
        };

        await FluentActions.Invoking(() =>
            SendAsync(command)).Should().ThrowAsync<ValidationException>();
    }

    [Test]
    public async Task ShouldCreateTodoList()
    {
        var command = new CreateArtistCommand
        {
            FirstName = "Tasks"
        };

        var id = await SendAsync(command);

        var list = await FindAsync<Artist>(id);

        list.Should().NotBeNull();
        list!.FirstName.Should().Be(command.FirstName);
        list.CreatedAt.Should().BeCloseTo(DateTime.Now, TimeSpan.FromMilliseconds(10000));
    }
}
