using FluentValidation;

namespace bejebeje.admin.Application.Lyrics.Commands.UpdateLyric;

public class UpdateLyricCommandValidator : AbstractValidator<UpdateLyricCommand>
{
    public UpdateLyricCommandValidator()
    {
        RuleFor(v => v.Title)
            .MaximumLength(200)
            .NotEmpty();
    }
}
