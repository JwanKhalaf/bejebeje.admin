using bejebeje.admin.Application.Common.Interfaces;
using FluentValidation;

namespace bejebeje.admin.Application.Artists.Commands.CreateArtist;

public class CreateArtistCommandValidator : AbstractValidator<CreateArtistCommand>
{
    private readonly IApplicationDbContext _context;

    public CreateArtistCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .MaximumLength(200).WithMessage("First name must not exceed 200 characters.");
    }
}
