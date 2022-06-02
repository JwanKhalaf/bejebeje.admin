using FluentValidation;

namespace bejebeje.admin.Application.Lyrics.Queries.GetLyrics;

public class GetLyricsWithPaginationQueryValidator : AbstractValidator<GetAllLyricsWithPaginationQuery>
{
    public GetLyricsWithPaginationQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1).WithMessage("PageNumber at least greater than or equal to 1.");

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1).WithMessage("PageSize at least greater than or equal to 1.");
    }
}
