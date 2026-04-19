using bejebeje.admin.Application.Artists.Commands.CreateArtist;
using bejebeje.admin.Application.Artists.Queries.GetArtists;
using NUnit.Framework;
using Shouldly;

namespace bejebeje.admin.Application.IntegrationTests.Artists.Queries;

using static Testing;

public class GetArtistsTests : TestBase
{
    [Test]
    public async Task ShouldReturnArtists()
    {
        await SendAsync(new CreateArtistCommand
        {
            FirstName = "Beytocan",
            Sex = "m"
        });

        var query = new GetAllArtistsWithPaginationQuery();

        var result = await SendAsync(query);

        result.Items.Count.ShouldBe(1);
    }
}
