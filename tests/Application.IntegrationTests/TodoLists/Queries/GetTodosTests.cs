using bejebeje.admin.Application.TodoLists.Queries.GetTodos;
using bejebeje.admin.Domain.Entities;
using bejebeje.admin.Domain.ValueObjects;
using FluentAssertions;
using NUnit.Framework;

namespace bejebeje.admin.Application.IntegrationTests.TodoLists.Queries;

using static Testing;

public class GetTodosTests : TestBase
{
    [Test]
    public async Task ShouldReturnPriorityLevels()
    {
        var query = new GetTodosQuery();

        var result = await SendAsync(query);

        result.PriorityLevels.Should().NotBeEmpty();
    }

    [Test]
    public async Task ShouldReturnAllListsAndItems()
    {
        await AddAsync(new Artist
        {
            Title = "Shopping",
            Colour = Colour.Blue,
            Items =
                    {
                        new Lyric { Title = "Apples", Done = true },
                        new Lyric { Title = "Milk", Done = true },
                        new Lyric { Title = "Bread", Done = true },
                        new Lyric { Title = "Toilet paper" },
                        new Lyric { Title = "Pasta" },
                        new Lyric { Title = "Tissues" },
                        new Lyric { Title = "Tuna" }
                    }
        });

        var query = new GetTodosQuery();

        var result = await SendAsync(query);

        result.Lists.Should().HaveCount(1);
        result.Lists.First().Items.Should().HaveCount(7);
    }
}
