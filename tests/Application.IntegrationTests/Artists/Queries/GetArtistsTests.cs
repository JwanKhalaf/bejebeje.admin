using bejebeje.admin.Application.Artists.Queries.GetArtists;
using bejebeje.admin.Domain.Entities;
using FluentAssertions;
using NUnit.Framework;

namespace bejebeje.admin.Application.IntegrationTests.Artists.Queries;

using static Testing;

public class GetArtistsTests : TestBase
{
    [Test]
    public async Task ShouldReturnPriorityLevels()
    {
        var query = new GetArtistsQuery();

        var result = await SendAsync(query);
    }

    [Test]
    public async Task ShouldReturnAllListsAndItems()
    {
        await AddAsync(new Artist
        {
            FirstName = "Shopping"
        });

        var query = new GetArtistsQuery();

        var result = await SendAsync(query);

        result.Artists.Should().HaveCount(1);
    }
}
