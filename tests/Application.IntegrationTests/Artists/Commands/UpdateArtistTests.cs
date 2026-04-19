using bejebeje.admin.Application.Artists.Commands.CreateArtist;
using bejebeje.admin.Application.Artists.Commands.UpdateArtist;
using bejebeje.admin.Application.Common.Exceptions;
using bejebeje.admin.Domain.Entities;
using NUnit.Framework;
using Shouldly;

namespace bejebeje.admin.Application.IntegrationTests.Artists.Commands;

using static Testing;

public class UpdateArtistTests : TestBase
{
    [Test]
    public async Task ShouldRequireValidTodoListId()
    {
        var command = new UpdateArtistCommand { Id = 99, FirstName = "New Title" };
        await Should.ThrowAsync<NotFoundException>(() => SendAsync(command));
    }

    [Test]
    [Ignore("no uniqueness validator for artist firstname — template residue; revisit if uniqueness is actually required")]
    public async Task ShouldRequireUniqueTitle()
    {
        var listId = await SendAsync(new CreateArtistCommand
        {
            FirstName = "New List",
            Sex = "m"
        });

        await SendAsync(new CreateArtistCommand
        {
            FirstName = "Other List",
            Sex = "m"
        });

        var command = new UpdateArtistCommand
        {
            Id = listId,
            FirstName = "Other List"
        };

        var exception = await Should.ThrowAsync<ValidationException>(() => SendAsync(command));
        exception.Errors.ShouldContainKey("Title");
        exception.Errors["Title"].ShouldContain("The specified title already exists.");
    }

    [Test]
    public async Task ShouldUpdateTodoList()
    {
        var listId = await SendAsync(new CreateArtistCommand
        {
            FirstName = "New List",
            Sex = "m"
        });

        var command = new UpdateArtistCommand
        {
            Id = listId,
            FirstName = "Updated List Title"
        };

        await SendAsync(command);

        var list = await FindAsync<Artist>(listId);

        list.ShouldNotBeNull();
        list.FirstName.ShouldBe(command.FirstName);
        list.ModifiedAt.ShouldNotBeNull();
        list.ModifiedAt.Value.ShouldBe(DateTime.UtcNow, TimeSpan.FromMilliseconds(10000));
    }
}
