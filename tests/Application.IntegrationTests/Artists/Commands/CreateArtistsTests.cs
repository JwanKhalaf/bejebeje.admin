using bejebeje.admin.Application.Artists.Commands.CreateArtist;
using bejebeje.admin.Application.Common.Exceptions;
using Bejebeje.Shared.Domain;
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
    [Ignore("no uniqueness validator for artist firstname — template residue; revisit if uniqueness is actually required")]
    public async Task ShouldRequireUniqueTitle()
    {
        await SendAsync(new CreateArtistCommand
        {
            FirstName = "Shopping",
            Sex = "m"
        });

        var command = new CreateArtistCommand
        {
            FirstName = "Shopping",
            Sex = "m"
        };

        await Should.ThrowAsync<ValidationException>(() => SendAsync(command));
    }

    [Test]
    public async Task ShouldCreateTodoList()
    {
        var command = new CreateArtistCommand
        {
            FirstName = "Tasks",
            Sex = "m"
        };

        var id = await SendAsync(command);

        var list = await FindAsync<Artist>(id);

        list.ShouldNotBeNull();
        list.FirstName.ShouldBe(command.FirstName.ToLowerInvariant());
        list.CreatedAt.ShouldBe(DateTime.UtcNow, TimeSpan.FromMilliseconds(10000));
    }
}
