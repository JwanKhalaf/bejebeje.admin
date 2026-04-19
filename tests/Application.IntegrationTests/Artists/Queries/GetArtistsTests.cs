using bejebeje.admin.Application.Artists.Queries.GetArtists;
using bejebeje.admin.Domain.Entities;
using NUnit.Framework;
using Shouldly;

namespace bejebeje.admin.Application.IntegrationTests.Artists.Queries;

using static Testing;

public class GetArtistsTests : TestBase
{
    [Test]
    public async Task ShouldReturnArtists()
    {
        await AddAsync(new Artist
        {
            FirstName = "Beytocan"
        });

        var query = new GetAllArtistsWithPaginationQuery();

        var result = await SendAsync(query);

        result.Items.Count.ShouldBe(1);
    }
}
