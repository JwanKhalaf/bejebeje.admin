using bejebeje.admin.Application.Artists.Commands.CreateArtist;
using bejebeje.admin.Application.Artists.Commands.UpdateArtist;
using bejebeje.admin.Application.Common.Exceptions;
using bejebeje.admin.Domain.Entities;
using FluentAssertions;
using NUnit.Framework;

namespace bejebeje.admin.Application.IntegrationTests.Artists.Commands;

using static Testing;

public class UpdateArtistTests : TestBase
{
    [Test]
    public async Task ShouldRequireValidTodoListId()
    {
        var command = new UpdateArtistCommand { Id = 99, FirstName = "New Title" };
        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<NotFoundException>();
    }

    [Test]
    public async Task ShouldRequireUniqueTitle()
    {
        var listId = await SendAsync(new CreateArtistCommand
        {
            FirstName = "New List"
        });

        await SendAsync(new CreateArtistCommand
        {
            FirstName = "Other List"
        });

        var command = new UpdateArtistCommand
        {
            Id = listId,
            FirstName = "Other List"
        };

        (await FluentActions.Invoking(() =>
            SendAsync(command))
                .Should().ThrowAsync<ValidationException>().Where(ex => ex.Errors.ContainsKey("Title")))
                .And.Errors["Title"].Should().Contain("The specified title already exists.");
    }

    [Test]
    public async Task ShouldUpdateTodoList()
    {
        var listId = await SendAsync(new CreateArtistCommand
        {
            FirstName = "New List"
        });

        var command = new UpdateArtistCommand
        {
            Id = listId,
            FirstName = "Updated List Title"
        };

        await SendAsync(command);

        var list = await FindAsync<Artist>(listId);

        list.Should().NotBeNull();
        list!.FirstName.Should().Be(command.FirstName);
        list.ModifiedAt.Should().NotBeNull();
        list.ModifiedAt.Should().BeCloseTo(DateTime.Now, TimeSpan.FromMilliseconds(10000));
    }
}
