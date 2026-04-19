using bejebeje.admin.Application.Artists.Commands.CreateArtist;
using bejebeje.admin.Application.Common.Exceptions;
using bejebeje.admin.Domain.Entities;
using NUnit.Framework;
using Shouldly;

namespace bejebeje.admin.Application.IntegrationTests.Artists.Commands;

using static Testing;

public class CreateArtistsTests : TestBase
{
    [Test]
    public async Task ShouldRequireMinimumFields()
    {
        var command = new CreateArtistCommand();
        await Should.ThrowAsync<ValidationException>(() => SendAsync(command));
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

        await Should.ThrowAsync<ValidationException>(() => SendAsync(command));
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

        list.ShouldNotBeNull();
        list.FirstName.ShouldBe(command.FirstName);
        list.CreatedAt.ShouldBe(DateTime.Now, TimeSpan.FromMilliseconds(10000));
    }
}
